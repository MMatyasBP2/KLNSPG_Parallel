# Párhuzamos algoritmusok beadandó feladatok specifikációi:

OpenMP: Numerikus integrációs módszerek összehasonlítása szekvenciális és párhuzamos futtatással. A projekt célja, hogy összehasonlítsa a különböző numerikus integrációs módszerek futási idejét szekvenciálisan és párhuzamosan, az "OpenMP" függvénykönyvtár használatával.
A program összesen 8 módszerrel dolgozik:
- Newton módszer,
- Taylor-sor első rendű,
- Taylor-sor másodrendű,
- Trapéz módszer középponti értékeléssel,
- Trapéz módszer bal végponti értékeléssel,
- Trapéz módszer jobb végponti értékeléssel,
- Simpson 1/3-ad szabály,
- Simpson 3/8-ad szabály. A módszereket 1 000 000-szor futtatjuk le, és a futási időket külön szövegfájlokban rögzítjük. A futási időket egy "Python script" segítségével ábrázoljuk. A "Python script" az eredményeket beolvassa, és grafikonon ábrázolja a szekvenciális és párhuzamos futási időket. A grafikonon az x tengely a numerikus integrációs módszereket mutatja, míg az y tengely a futási időt másodpercben.

POSIX: Autóverseny szimuláció párhuzamosítással. A projekt célja egy egyszerű autóverseny szimuláció létrehozása "C" programozási nyelven, melyben a párhuzamos programozás előnyeit mutatjuk be. A szimuláció során több autó versenyez egymással, és mindegyik autó egy szálat kap a verseny alatt. A szimuláció végén kiírjuk a verseny rangsorát, és összehasonlítjuk a több szálas és egy szálas futtatás idejét. A programban 10 autó vesz részt a versenyen. Az autók azonosítója 1-től 10-ig tart. Az autóverseny távolsága 10 000 egység. Az autók a verseny során véletlenszerűen haladnak előre (1-50 egység között), és eltöltött idő is véletlenszerű (1-1 000 időegység között). A verseny addig tart, amíg az összes autó el nem éri a maximális távolságot.

C#: Statisztikai mintavételezés párhuzamosítás használatával.
    A projekt célja az egyszerű statisztikai mintavételezés párhuzamos implementálása. A program egyszerre egy időben végzi el a statisztikai mintavételezést és a
    hisztogrammmok kirajzolásást. Mindkettő funkciót egy-egy gombbal lehet elérni. Egy egyszerű "WindowsFormos" applikációban valósítjuk meg az alábbiakat. A teljes mintavételezés és kiíratás egy aszinkron függvénnyel működik és van egy
    "CancellationTokenSource"-a, ami a Stop gomb megnyomására aktiválódik. A program futása folyamán egy led lámpa villog, ezt két kép folyamatosan felcserélgetésével
    oldjuk meg és a "System.Threading" függvénykönyvtárat használjuk fel hozzá. Egy új szálat hozunk létre, ami egy végtelen ciklust indít el és folyamatosan 250
    miliszekundumot várunk minden egyes képváltásnál. A képek váltása a "Dispacher" osztállyal működik és az "Invoke" metódus felel azért, hogy hozzáférjünk a kép aktuális száljához. Az Open configuration file gomb felhív egy "JSON" fájlt, amiben megadhatjuk a mintavételezés méretét. A program is ebből a fájlból olvas. A futás után egy másik ablakban jelennek meg a futási eredmények (ezeket ki is írja fájlba a program), illetve egy harmadik ablakban, hogy mennyi időt vett volna igénybe az egyszálon történő futás és mennyi időt vett igénybe a több szálú futás.
