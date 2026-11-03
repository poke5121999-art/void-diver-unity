-- VOID DIVER web log uploads (the page's log collector, Assets/_VD/Editor/vd-log-collector.js.txt).
-- Paste once into the Supabase SQL editor of the hub project. The anon key may insert and read; nothing else.
create table if not exists public.vd_logs (
  id bigint generated always as identity primary key,
  created_at timestamptz not null default now(),
  session text,
  build text,
  trigger text,
  note text,
  url text,
  ua text,
  buffer text check (char_length(buffer) <= 2000000)
);
create index if not exists vd_logs_created_at on public.vd_logs (created_at desc);
create index if not exists vd_logs_session on public.vd_logs (session);
alter table public.vd_logs enable row level security;
drop policy if exists vd_logs_anon_insert on public.vd_logs;
create policy vd_logs_anon_insert on public.vd_logs for insert to anon with check (true);
drop policy if exists vd_logs_anon_select on public.vd_logs;
create policy vd_logs_anon_select on public.vd_logs for select to anon using (true);
grant insert, select on public.vd_logs to anon;
