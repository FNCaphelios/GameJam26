# Codedokumentation
Jede einzelne Funktion sollte über der Definition als Kommentar die gewünschte Funktionsweise beschreiben.
Dabei gibt es einen goldenen Standard

## XML-Dokumentationskommentare
Vergleichbar zur funktionsweise von JavaDocs benutzt C# eine XML Struktur für Codedokumentation. Dabei gibt es folgende Struktur:
- /// <summary Kurze Beschreibung der Funktion </summary
- /// <param name="a" Kurze Beschreibung des Parameters </param
- /// <returns Kurze Beschreibung der Rückgabe </returns

 Ein gut dokumentierter Code hat an jeder neuen Definition solche Kommentare. Das heißt alle Klassen-, Werte- und Funktionsdefinitionen.
 Der Vorteil: Diese Struktur ermöglicht es uns, automatisch eine html-Codedokumentation zu führen, welche für das langfristige Arbeiten mit dem Code sehr hilfreich ist.
 Zudem besitzen IDEs wie Visual Code oft eine automatische Erkennung. Man kann also an anderen Stellen über die Aufrufe hovern und es wird direkt die Dokumentation angezeigt.

 ## Mindestanforderung
 Da eine solche Dokumentation jedoch aufwendig ist, kann nicht erwartet werden, dass alles diesem Standard direkt folgt.
 Code sollte jedoch von Beginn an Kommentare besitzen, welche die grobe Funktionsweise der Funktion/Klasse erklärt.
 Dafür reicht ein einfacher Kommentar schon aus.

 # Codeconvention

Code sollte komplett auf englisch gehalten werden
Code sollte den Visual Code Conventionsempfehlungen folgen

## Skriptbenennung

Skripts sollten nach folgenden Richtlinien benannt werden:
*BetreffAufgabe*

Betreff: Für welche Objekte gilt das Skript. Bsp: NPC, Player, Item,...
Aufgabe: Was für eine Aufgabe erfüllt es. Bsp: InteractionHandler, Movement,...
