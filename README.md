# Solarertrag / Zählerstand

![NET](https://img.shields.io/badge/NET-10.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2026-white.svg)
![Version](https://img.shields.io/badge/Version-1.3.2025.11-yellow.svg)

das ist eine kleine Applikation in NET 7 und WPF. Mit dieser kleinen Applikation werden verschiedene Konzepte aus NET 7 / WPF verwendet und gegen das .Net Framework Classic geprüft. Ziel soll sein, in wie weit beide .Net Varianten Code kompatibel sind. Die Applikation wird mit einem eigenen Entwicklungsframework "EasyPrototypingNET" das sich bisher  selbst noch in einem frühen Entwicklungsstatium befindet.

Aufgabe der kleinen Applikation soll sein, für jeden Tag den Tagesertrag einer Solaranlage zu erfassen. Die Daten werden in einer NoSql Datenbank (LiteDB) gespeichert und können dann vielfältig ausgewertet werden.</br>
Zusätzlich kann nun in einem eigenen Dialog auch der Zählerstand für jeden einzelnen Tag erfallst werden. Über eine Übersicht werden die Zählerstänge gruppiert pro Jahr eingezeigt. Von der Übersicht aus können einzelne Einträge bearbeitet oder gelöscht werden.

In dem Projekt wird noch ein zusätzliche Pakete verwendet

| Paket | Beschreibung |
|:---:|:---:|
| [LiteDB](https://github.com/mbdavid/LiteDB) | NoSQL Datenbank |

Das kleine Programm ist in einer Monolithische Architekr als "Singel-Page-Application" erstellt. Für die Applikation selbst gibt es nur eine ausführbares Programm ohne weitere Aufteilung der Fuktionen.

![Version](https://img.shields.io/badge/Version-1.3.2025.16-yellow.svg)</br>
- Auswertungs Chart für Jahr und Monat

![Version](https://img.shields.io/badge/Version-1.3.2025.11-yellow.svg)</br>
- Wechsel auf NET 10

![Version](https://img.shields.io/badge/Version-1.2.2025.10-yellow.svg)</br>
- Übersichtsdialog zum Zählerstand mit Such- und Gruppierungsfunktion
- Erweiterung Excel-Export für Zählerstand
- Weitere Fehlerbehebung

![Version](https://img.shields.io/badge/Version-1.1.2025.6-yellow.svg)</br>
- Dialog zum erfassen des Zählerstand
- Weitere Fehlerbehebung
