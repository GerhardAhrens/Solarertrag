echo off
set packagename=SolarertragNET
set versioninfo=1.0.2025.5
set developer=Lifeprojects.de
rem "c:\Program Files\dotnet\dotnet.exe" list Inventar.sln  package --vulnerable --include-transitive
rem -m <Verzeichnisname> für eigenes Verzeichnis der spdx Datei
sbom generate -bc c:\_Projekte\_Git_Private\Solarertrag\ -b c:\_Projekte\_Git_Private\Solarertrag\ -pn %packagename% -ps %developer% -pv %versioninfo% -li true -pm true -mi SPDX:2.2 -D true  -V Information
pause