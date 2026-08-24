# Branches

Insgesamt werden 3 Arten von branches geben.
  1. main: Auf diesem wird NICHT direkt entwickelt, sondern er dient zum abzweigen von dem aktuellen Entwicklungsstand
  2. release: Release branches werden genutzt, um spielfähige Versionen zu behalten. Jede Version wird entsprechend getagged und markiert.
  3. development: Hier wird eigentlich erst gearbeitet.

## Namensgebung

Ein Development Branch sollte wie folgt benannt werden:
Prefix/Release/(Issue)/Name

### Prefixes:
  Prefixes beschreiben kurz die Art des branches, dabei unterscheiden wir unter folgenden:
    - *feature* - Neue Funktionen werden zu dem Spiel hinzugefügt
    - *refactor* - Bestehender Code wird rein visuell angepasst, es ändert sich nichts an der Logik des Codes
    - *debug* - Hier wird ungewolltes Spielverhalten behoben
    - *doc* - Hier können ausbauten von Code vorgenommen werden

### Release

Unser Ziel sollte es sein, das Game innerhalb von Vollversionen spielfähig zu halten. Die Release Nummer wird im späteren Verlauf helfen, den Kontext des branches zu erkennen

Aufbau:
  Wir nehmen drei Zahlen, die folgende Bedeutung haben:
  1. Vollversion (die erste Vollversion sollte die Gamejam-Abgabe sein)
  2. Unterversion (es werden nach Release einer Vollversion mit Sicherheit Probleme aufkommen, die wir mittles     Unterversionen fixen können
  3. Sprintnummer (Hier wird angegeben, in welchem Sprint wir aktuell arbeiten

Beispiel also wäre aktuell:
.../0.0.0/... (preRelease-ohne Unterversion-nullter Sprint)

### Issue

Bugfixes werden standardmäßig über das Issueboard verlinkt. Sollte an einem Bug aus dem Issueboard gearbeitet werden, sollte die Nummer hier angegeben werden.
Das ist jedoch nur in diesem Fall nötig und kann sonst ausgelassen werden

### Name

Der Name sollte auf englisch geschrieben werden und sehr kurz die Änderung beschreiben.
Der Name wird im Präsens verfasst und sollte auch im Camelcase geschrieben werden.
