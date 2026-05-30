CopyWallpapers
===============

Kurzbeschreibung

CopyWallpapers kopiert hochwertige Windows Spotlight / Hintergrundbilder aus dem lokalen Windows-Content-Delivery-Ordner in einen vom Anwender gewählten Zielordner und benennt die Dateien in JPG um.

Wichtige Änderungen (aktualisiert)

- UI: Zielordner-Auswahl per FolderBrowserDialog, Formular zentriert, Mindestgröße gesetzt
- Progress: ProgressBar und Status-Label hinzugefügt
- Asynchron: Kopiervorgang läuft asynchron (Task.Run) und berichtet Fortschritt per IProgress<int>
- File-Lock-Vermeidung: Bilder werden in einen MemoryStream geladen, bevor geprüft/gespeichert wird
- Temp-Ordner: eindeutiger Temp-Ordner (GUID) und Löschung im finally-Block

Schnellstart (Entwicklungsumgebung)

1. Öffnen Sie die Lösung in Visual Studio (empfohlen: Visual Studio 2022/2026).
2. Projekt-Target: .NET Framework 4.7.2 (bereits gesetzt).
3. Build (Strg+Shift+B) und Starten (F5).

Benutzung

1. "..."-Button drücken, Zielordner auswählen.
2. Auf "Download" klicken. Der Fortschritt wird in der ProgressBar angezeigt.
3. Nach Abschluss erscheint eine Erfolgsmeldung mit der Anzahl neuer Bilder.

Bekannte Einschränkungen / ToDo

- Kein Abbrechen während des Kopiervorgangs (Cancel-Token noch nicht implementiert).
- Einstellungen (z. B. letzter Zielpfad) werden noch nicht persistent gespeichert.
- Weitere Verbesserungen möglich: WallpaperService extrahieren, Logging, erweiterte Fehlerbehandlung und Unit-Tests.

Hinweis zur Verwendung

Dieses Tool liest Dateien aus dem Benutzerprofil (LocalApplicationData). Bitte prüfen Sie, ob ausreichende Zugriffsrechte bestehen.

Mitwirkende

- Originalprojekt: https://github.com/Prophisor/copy-windows-wallpapers
- Änderungen implementiert von: GitHub Copilot

Lizenz

Standardeigentümer-Lizenz des Originalprojekts beachten (Repo). Keine Lizenzdatei hier erstellt.