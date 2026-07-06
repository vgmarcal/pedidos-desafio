@echo off
rem Sobe o dev server do Angular usando o Node 24 portatil (o Node global e v14, incompativel com Angular 22)
set "PATH=C:\PROJETOS\tools\node24;%PATH%"
cd /d C:\PROJETOS\pedidos-web
npm start
