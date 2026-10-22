#!/usr/bin/env python3
"""Fills Parse/ToCsv/Pk/Load* in every GameCommon.Table.T*.cs from its property list and the table CSV header.

Columns come from Assets/StreamingAssets/TableEncrypted (XOR 0xCC, or plain .csv). Rewrites files in place and is
idempotent: only method bodies it owns are replaced. Usage: python3 tools/codegen/table_parse.py [--check]
"""
import csv
import io
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
TABLE_DIR = ROOT / "Assets/Scripts/De.Base/GameCommon/Table"
SCRIPTS_DIR = ROOT / "Assets/Scripts"
DATA_DIR = ROOT / "Assets/StreamingAssets/TableEncrypted"
XOR_KEY = 0xCC

SCALAR = {
    "int": "TableParse.Int({c})",
    "long": "TableParse.Long({c})",
    "byte": "TableParse.Byte({c})",
    "float": "TableParse.Float({c})",
    "bool": "TableParse.Bool({c})",
    "string": "TableParse.Str({c})",
}

# Row field feeding each GroupTable (GroupBy key; groups are ordered by Pk) and each SubTable (key tuple).
# Read from the lambdas of the original LoadFromText bodies.
GROUP_KEYS = {
    "TAreaDecoration": "GroupId", "TCampaign": "Category", "TCampaignTask": "CampaignId",
    "TCharacterSkin": "CharacterId", "TCharacterSkinPreset": "CharacterSkinId", "TCrafting": "GroupId",
    "TDialogReplay": "QuestTableId", "TDropReward": "GroupId", "TDropRewardArtifactProbability": "DropRewardId",
    "TDropRewardProbability": "DropRewardId", "TEquipmentEffect": "GroupId", "TEquipmentSet": "GroupId",
    "TGlossary": "Type", "TInputAction": "ControlDevice", "TInteriorShop": "AreaId",
    "TLoadingText": "LoadingPopupCondition", "TLoungeQuestTask": "LoungeQuestId", "TMonster": "GroupId",
    "TMonsterSpawn": "GroupId", "TNpcFunction": "NpcId", "TParadox": "Level", "TRewardBox": "GroupId",
    "TShopProduct": "GroupId", "TTalent": "GroupId",
}
SUB_KEYS = {
    "TEquipmentEffect": ("GroupId", "Level"), "TEquipmentSet": ("GroupId", "Count"),
    "TParadoxLevel": ("Level", "IsPositive"), "TShadowMonsterSpawn": ("Difficulty", "StressState"),
    "TTalent": ("BitFlagIndex",),
}

CLASS_RE = re.compile(r"public class (T\w+) : ITableData<(\w+)>")
PROP_RE = re.compile(r"\n\t\tpublic (?!static )([\w\[\]<>, ]+?) (\w+) \{ get; set; \}")
TABLE_PROP_RE = re.compile(r"public static (\w+) Table \{ get; set; \}")


def read_header(name):
    path = DATA_DIR / (name + ".bytes")
    if path.exists():
        text = bytes(b ^ XOR_KEY for b in path.read_bytes()).decode("utf-8-sig")
    else:
        text = (DATA_DIR / (name + ".csv")).read_text(encoding="utf-8-sig")
    rows = list(csv.reader(io.StringIO(text, newline="")))
    return rows[0], rows[1]


def lit(text):
    return '"' + text.replace("\\", "\\\\").replace('"', '\\"') + '"'


_enums = None


def is_enum(type_name):
    global _enums
    if _enums is None:
        _enums = set()
        for source in SCRIPTS_DIR.rglob("E*.cs"):
            _enums.update(re.findall(r"\benum (\w+)", source.read_text(encoding="utf-8", errors="ignore")))
    return type_name in _enums


def parse_expr(type_name, cell):
    if type_name in SCALAR:
        return SCALAR[type_name].format(c=cell)
    if is_enum(type_name):
        return f"TableParse.Enum<{type_name}>({cell})"
    if type_name.endswith("[]"):
        element = type_name[:-2]
        if element in SCALAR or is_enum(element):
            return f"TableParse.Array<{element}>({cell})"
    return f"TableParse.Json<{type_name}>({cell})"


def find_block(text, start):
    """Index just past the brace block that opens at or after `start`."""
    open_at = text.index("{", start)
    depth = 0
    for i in range(open_at, len(text)):
        if text[i] == "{":
            depth += 1
        elif text[i] == "}":
            depth -= 1
            if depth == 0:
                return open_at, i + 1
    raise ValueError("unbalanced braces")


def replace_body(text, signature, body_lines):
    at = text.find(signature)
    if at < 0:
        raise ValueError("missing " + signature)
    open_at, end = find_block(text, at)
    body = "".join("\t\t\t" + line + "\n" if line else "\n" for line in body_lines)
    return text[:open_at] + "{\n" + body + "\t\t}" + text[end:]


def generate(path):
    text = path.read_text(encoding="utf-8")
    cls = CLASS_RE.search(text)
    if not cls:
        return None
    class_name, key_type = cls.groups()
    table_name = class_name[1:]
    props = [(t.strip(), n) for t, n in PROP_RE.findall(text) if n != "OnDataLoaded"]
    names, types = read_header(table_name)
    keep = [i for i, n in enumerate(names) if not n.startswith("~")]
    columns = [names[i] for i in keep]
    base_names = [c.split(":")[0].strip() for c in columns]
    if base_names != [n for _, n in props]:
        raise SystemExit(f"{class_name}: properties {[n for _, n in props]} do not match columns {base_names}")
    table_type = TABLE_PROP_RE.search(text).group(1)

    parse = [f"{n} = {parse_expr(t, f'columns[{i}]')};" for i, (t, n) in enumerate(props)]
    text = replace_body(text, "public void Parse(string[] columns)", parse)
    to_csv = ["return TableParse.ToCsv(" + ", ".join(n for _, n in props) + ");"]
    text = replace_body(text, "public string ToCsv()", to_csv)
    text = replace_body(text, "public static string GetColumnsCsv()", [f"return {lit(','.join(columns))};"])
    text = replace_body(text, "public static string GetTypesCsv()", [f"return {lit(','.join(types[i] for i in keep))};"])
    text = replace_body(text, "public static void SaveCsv(string filePath)",
                        ['global::System.IO.File.WriteAllText(filePath, GetColumnsCsv() + "\\n" + GetTypesCsv() + "\\n" + Table.ToCsv());'])
    text = replace_body(text, "public static void LoadFromPath(string filePath)",
                        ["LoadFromText(global::System.IO.File.ReadAllText(filePath));"])
    load = [
        f"var table = new {table_type}();",
        f"table.Load(new CsvTableLoader<{key_type}, {class_name}>(text));",
        "Table = table;",
    ]
    if class_name in GROUP_KEYS:
        group_type = re.search(r"public static (\w+) GroupTable \{ get; set; \}", text).group(1)
        load += [
            f"GroupTable = new {group_type}();",
            f"GroupTable.Init(table.Values.GroupBy(row => row.{GROUP_KEYS[class_name]}).ToDictionary(group => group.Key, group => group.OrderBy(row => row.Pk).ToArray()));",
        ]
    if class_name in SUB_KEYS:
        sub_type = re.search(r"public static (\w+) SubTable \{ get; set; \}", text).group(1)
        keys = ", ".join(f"row.{k}" for k in SUB_KEYS[class_name])
        load += [f"SubTable = new {sub_type}();", f"SubTable.Init(table.Values.Select(row => ({keys}, row)));"]
    load.append("InvokeTableChangeEvent();")
    text = replace_body(text, "public static void LoadFromText(string text)", load)
    uses_linq = class_name in GROUP_KEYS or class_name in SUB_KEYS
    if uses_linq and "using System.Linq;" not in text:
        text = text.replace("using System;\n", "using System;\nusing System.Linq;\n", 1)
    elif not uses_linq:
        text = text.replace("using System.Linq;\n", "", 1)
    text = replace_body(text, "public static void InvokeTableChangeEvent()", ["m_OnTableChanged?.Invoke();"])

    event_at = text.index("public static event Action OnTableChanged")
    _, event_end = find_block(text, event_at)
    event_text = (
        "public static event Action OnTableChanged\n\t\t{\n"
        "\t\t\t[CompilerGenerated]\n\t\t\tadd\n\t\t\t{\n\t\t\t\tm_OnTableChanged += value;\n\t\t\t}\n"
        "\t\t\t[CompilerGenerated]\n\t\t\tremove\n\t\t\t{\n\t\t\t\tm_OnTableChanged -= value;\n\t\t\t}\n\t\t}"
    )
    text = text[:event_at] + event_text + text[event_end:]

    pk_at = text.index(f"public {key_type} Pk")
    get_at = text.index("get", pk_at)
    open_at, end = find_block(text, get_at)
    text = text[:open_at] + "{\n\t\t\t\treturn " + props[0][1] + ";\n\t\t\t}" + text[end:]
    return text


def main():
    check = "--check" in sys.argv
    changed = 0
    for path in sorted(TABLE_DIR.glob("T*.cs")):
        new = generate(path)
        if new is None:
            continue
        if new != path.read_text(encoding="utf-8"):
            changed += 1
            if not check:
                path.write_text(new, encoding="utf-8", newline="")
    print(f"{'would change' if check else 'changed'} {changed} files")
    return 1 if check and changed else 0


if __name__ == "__main__":
    sys.exit(main())
