# Solarertrag (for NET 7)
das ist eine kleine Applikation in NET 7 und WPF. Mit dieser kleinen Applikation werden verschiedene Konzepte aus NET 7 / WPF verwendet und gegen das .Net Framework Classic geprüft. Ziel soll sein, in wie weit beide .Net Varianten Code kompatibel sind. Die Applikation wird mit einem eigenen Entwicklungsframework "EasyPrototypingNET" das sich bisher  selbst noch in einem frühen Entwicklungsstatium befindet.

Aufgabe der kleinen Applikation soll sein, für jeden Tag den Tagesertrag einer Solaranlage zu erfassen. Die Daten werden in einer NoSql Datenbank (LiteDB) gespeichert und können dann vielfältig ausgewertet werden. 

In dem Projekt wird noch ein zusätzliche Pakete verwendet
| Paket | Beschreibung |
|:---:|:---:|
| [LiteDB](https://github.com/mbdavid/LiteDB) | NoSQL Datenbank |

```xml
<PublishSingleFile>true</PublishSingleFile>
<SelfContained>true</SelfContained>
<EnableCompressionInSingleFile>true</EnableCompressionInSingleFile>
<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>
´´´
