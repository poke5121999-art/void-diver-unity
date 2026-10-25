# Fixtures

`saves-post-tutorial/` is the profile a real tutorial run leaves. A4_TutorialFlow asserts the lounge and campaign 1100
cleared in SavesDir, then copies SavesDir/*.sav to captures/review/a4-saves/; `refresh-post-tutorial.sh` copies those
over this folder and prints their sha256. A3, A5, A6 and A7 start from it. The `.json` files are readable mirrors and are not refreshed.
