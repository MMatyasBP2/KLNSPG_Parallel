# Párhuzamos algoritmusok beadandó feladatok specifikációi:

OpenMP: Numerikus integrációs módszerek összehasonlítása szekvenciális és párhuzamos végrehajtással. A projekt célja a különböző numerikus integrációs módszerek futási idejének szekvenciális és párhuzamos összehasonlítása az OpenMP könyvtár segítségével. A program összesen 8 módszert alkalmaz: - Newton-módszer, - Taylor sorozat elsőrendű, - Taylor sorozat másodrendű, - trapéz módszer középpont kiértékeléssel, - trapéz módszer bal végpont értékeléssel, - jobb végpont értékelés Trapéz módszer, - Simpson 1/3-ad rule, - Simpson's 3/8-ad rule Ezeket a metódusokat 1 000 000 alkalommal futtattuk le, és a futási időket külön szöveges fájlokban rögzítették. A futási időket Python-szkript segítségével ábrázoljuk.  A Python-szkript beolvassa az eredményeket, és grafikont készít a szekvenciális és párhuzamos futási időkről. A grafikonon az x tengely a numerikus integrációs módszert, míg az y tengely a futásidőt mutatja másodpercben.

POSIX: Versenyszimuláció párhuzamosítással.
 A projekt célja egy egyszerű versenyszimuláció létrehozása C programozási nyelven, ahol bemutatjuk a párhuzamos programozás előnyeit.a szimuláció során
 Több autó versenyez egymással, és mindegyik autónak van nyoma a versenyben.A szimuláció végén megírjuk a játék rangsorát a többszálúhoz képest
 és a szál végrehajtási ideje. A műsorban 10 autó indul a versenyen. 1-től 10-ig számozott autók. Az autó távolsága 10 000 egység.ez
 Az autó véletlenszerűen halad előre a verseny során (1-50 egység között), és véletlenszerű ideig (1-1000 időegység között) telik el.ig tart a játék
Nem minden autó képes elérni a maximális távolságot.

C#: Statisztikai mintavételezés párhuzamosítás használatával.
    A projekt célja az egyszerű statisztikai mintavételezés párhuzamos implementálása. A program egyszerre egy időben végzi el a statisztikai mintavételezést és a
    hisztogrammmok kirajzolásást. Mindkettő funkciót egy-egy gombbal lehet elérni. Egy egyszerű WindowsFormos applikációban valósítjuk meg az alábbiakat. A teljes mintavételezés és kiíratás egy aszinkron függvénnyel működik és van egy
    CancellationTokenSource-ja, ami a Stop gomb megnyomására aktiválódik. A program futása folyamán egy led lámpa villog, ezt két kép folyamatosan felcserélgetésével
    oldjuk meg és a System.Threading függvénykönyvtárat használjuk fel hozzá. Egy új szálat hozunk létre, ami egy végtelen ciklust indít el és folyamatosan 250
    miliszekundumot várunk minden egyes képváltásnál. A képek váltása a Dispacher osztállyal működik és az Invoke metódus felel azért, hogy hozzáférjünk a kép aktuális
    száljához.
