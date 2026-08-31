# Branches

Insgesamt werden 3 Arten von branches geben.
  1. main: Auf diesem wird NICHT direkt entwickelt, sondern er dient zum abzweigen von dem aktuellen Entwicklungsstand
  2. release: Release branches werden genutzt, um spielfähige Versionen zu behalten. Jede Version wird entsprechend getagged und markiert.
  3. coding branches: Hier passiert das eigentliche Arbeiten am Code

## Namensgebung

Ein Development Branch sollte wie folgt benannt werden:
Prefix/Release/(Issue)/Name

## Beispielenamen für Branches

feature/1.0.3/implementedMusicManager

feature/1.3.1/createdNewHouseAssests

debug/1.4.2/#15/removedTerminalDropBug

refactor/2.0.0/renamedClasses


### Prefixes:
  Prefixes beschreiben kurz die Art des branches, dabei unterscheiden wir unter folgenden:
  
- *feature* - Neue Funktionen werden zu dem Spiel hinzugefügt
- *refactor* - Bestehender Code wird rein visuell angepasst, es ändert sich nichts an der Logik des Codes
- *debug* - Hier wird ungewolltes Spielverhalten behoben
- *doc* - Hier können ausbauten von Code vorgenommen werden

### Releasenummer

Releasenummern dienen zur zeitlichen Einordnung der jeweiligen Branches. Diese werden helfen, die Branches jeweilig sauber zu halten und nachzuvollziehen, wann welcher Branch erstellt wurde und wann diese gelöscht werden können.

Aufbau:
  Wir nehmen drei Zahlen, die folgende Bedeutung haben:
  1. Vollversion (die erste Vollversion ist die Abgabe der Gamejam, die nächsten nach Absprache)
  2. Unterversion (In Unterversionen sollen Teilaspekte der Vollversionen funktionsfähig sein. Ebenfalls dienen sie den Bugfixes für vorherige Vollversionen)
  3. Sprint/Meetingsnummer (Gibt an, nach welchem Meeting der Branch erstellt wurde. Die Zählweise ist immer relativ zu den Vollversionen)

Beispiel also wäre aktuell:
.../1.2.3/... - Branch NACH der ersten Vollversion, NACH der zweiten Unterversion, NACH dem dritten Meeting nach Release der ersten Vollversion

*Anmerkung* Bitte achtet genau auf die Releasenummer, damit später es beim Löschen von Branches zu keinen Missverständnissen kommt.

#### Aktuelle Releasenr.: 1.1.1

### Issue

Bugfixes werden standardmäßig über das Issueboard verlinkt. Sollte an einem Bug aus dem Issueboard gearbeitet werden, muss die Nummer hier angegeben werden.
Die Angabe beschränkt sich jedoch *ausschließlich* auf das Arbeiten mit Issues aus dem Board. Sollten dies nicht der Fall sein, wird dieser Teil einfach weggelassen.

### Name

Der Name sollte im Präsenz geschrieben sein (commits in der Vergangenheitsform).
Der Name sollte mit einem Verb (möglichst nicht "added") beginnen
Der Name sollte auf englisch geschrieben werden und sehr kurz die Änderung beschreiben.
Der Name wird im Präsens verfasst und sollte auch im Camelcase geschrieben werden.
-> dasIstCamelCase, hierEinWeiteresBeispiel

Beispiel für Namen:
- removingBug
- creatingGravity
- expandingPlanets
- remodelingAssets
- makingNewMovement

## Branchlöschung

Spätestens zu Vollversionen werden branches gelöscht, damit wir unsere Umgebung nicht unnötig belasten.
