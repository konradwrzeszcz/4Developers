Link to Prezi: https://prezi.com/view/R6UfCQHMzaC1m4Ejdfom/

# WHOAMI
Cześć, jestem Konrad Wrzeszcz. Mam 5 lat doświadczenia w .NET. Pracuję w software housie Skyrise.Tech w Katowicach. Gdyby kogoś interesował kod, macie link poniżej.
# Agenda
Co to ta funkcyjność i w czym mi może pomóc?</br>
Opowiem wam dziś o tym

1. jakie zauważyliśmy problemy w standardowym podejściu pisania apek opartych o AspNecie, które nagminnie pojawiały się w całym naszym systemie

2. jak je zaadresowaliśmy stosując niektóre techniki z programowania funkcyjnego

3. podsumujemy naszą 2 letnia pracę, zobaczymy wtedy co w naszym podejściu działa, a co nie

4. przejrzymy dalsze plany rozwoju

5. no i podrzuce wam jakieś materiały, gdybyście się chcieli rozejrzeć w tym temacie

# Wstęp
We wstępie chciałbym powiedzieć wam, czym prezentacja nie jest

1. Po pierwsze nie jest zlepkiem definicji z wikipedii - poniewaz papier przyjmie wszystko, a tak naprawdę, każdy chce zobaczyć jak się to sprawuje w akcji. Liźniemy trochę teorii, ale tylko po wierzchu.

2. Po drugie, nie będzie to streszczenie poradnika dla poczatkujacych - poniewaz nienawidzę takich prezentacji - sam sobie umiem coś wpisać w Google

3. Po trzecie, nie będzie to gloryfikacja programowania funkcyjnego - bo tak jak wszystko na świecie oprócz plusów dodatnich, posiada także plusy ujemne. Tak naprawdę zarówno paradygmat funkcyjny, obiektowy jak i proceduralny są ze sobą tożsame. Tzn. program napisany w dowolnym paradygmacie można przepisać na inny paradygmat. Jeśli kogoś interesuje ten temat polecam poczytać o “kompletności Turinga” oraz o “hipotezie Churcha-Turinga”.

# Problemy
Dla zobrazowania problemu stworzyłem aplikację, która mniej więcej obrazuje standardowe podejście pisania kodu w firmie klienta.</br>
Jest to biuro podróży, gdzie użytkownik klika sobie w daną podróż i z backendu zwracane są szczegółowe dane podróży oraz wyliczane są zniżki, które użytkownik może wykorzystać.
Standardowo wydzieliłem logikę do obiektów - w naszym przypadku są to discountery (albo bardziej po naszemu - zniżkacze). Nasze zniżkacze są odpowiedzialne za wyliczenie zniżek z kuponu, last minute, albo lojalnościowej. Dodatkowo są obiekty dostarczające dane - DateTimeProvider do obecnego czasu oraz TravelProvider do zapisywania podróży w bazie danych, oraz obiekt pomocniczy Mapper służący do mapowania DTO.</br>
Więc jakie problemy zauważyliśmy w takim pisaniu kodu:

## Morze abstrakcji
Pierwszym problemem jest morze abstrakcji, które pojawia się zawsze

1. Chcemy zawsze wszystko wydzielać do osobnych abstrakcji, bo wiadomo - obiekt powinien mieć tylko jedną odpowiedzialność, ale które abstrakcje są poprawne i bardziej opisują rzeczywisty świat? Gdzie powinna być logika wyliczania zniżek? W jakimś kalkulatorze zniżek? Czy jednak powinno być X obiektów zniżkaczy? Czy może powinny być kalkulator, który ma zależność na obiekty zniżkaczy?</br>
Inny problem. Który obiekt powinien mieć logikę mapowania ten z bazy danych, czy ten z kontraktu API? Pewnie żaden, to gdzie ją dać? Musimy stworzyć dodatkową abstrakcję, która robi właśnie to.

2. Z drugiej strony, czym więcej zrobimy obiektów tym bardziej nasza logika będzie rozdzielona, co przy debugwaniu, a nawet czytaniu sprawia wielką trudność. Bo nigdy nie jesteś pewien, czy to już cała logika.

3. Dodatkowo, z powodu tego, że obiektu, który enkapsuluje swój stan nie da się przetestować, trzeba używać dependency injection, czyli wstrzyknąć stan. Nie chcemy jednak tworzyć zależności od konkretnej implementacji, bo będziemy musieli w każdym teście tworzyć ten konkretny obiekt, który może np używać konkretnej bazy danych. Przez co przyjęło się, że każda klasa implementuje interfejs IKlasa, dzięki czemu możemy w łatwy zamokować implementację danego obiektu w testach. Więc jak widać na załączonym obrazku, każdy obiekt ma dodatkowe 50 linii kodu, który nic nie robi i instnieje tylko po to, żeby móc sprawdzić w testach, czy nasz kod działa.

## Nieczytelny kod
Drugim problemem jest to, że kod napisany obiektowo jest imperatywny, tzn. jest on napisany z perspektywy komputera. 

1. Czyli, podczas pisania kodu muszę się zastanawiać JAK osiągnąć daną rzecz, zamiast skupić się na tym CO chce osiągnąć.

2. Dodatkowo gdy wrócimy do danego kodu, bo coś w nim nie działa musimy w głowie debugować co tak naprawdę się w nim dzieje. przez co na każdej pętli musimy czytać do pewnego momentu i wracać wzrokiem na górę kodu, na następna iterację. - Wchodzimy do naszej metody liczenia zniżki lojalnościowej, która polega na sprawdzeniu, czy użytkownik kupił w zeszłym roku więcej niż 3 wycieczki. Na górze widzimy jakieś zmienne pomocnicze, o których musimy pamiętać. Potem sprawdzamy kolejne warunki i wracamy na górę, albo wychodzimy z pętli. Zupełnie nienaturalny sposób czytania.   

## Trudne testowanie
Kolejną sprawą, o której już wspomniałem jest trudność testowania programowania obiektowego.

1. Każda klasa musi implementować interfejs IKlasa, jest to mnóstwo dodatkowego kodu

2. Zależności musimy jakoś zamockować, za pomocą zewnętrznej biblioteki, jak w przykładzie, albo ręcznie. Po czym jak wchodzimy w ciało naszego testu to tak naprawde połowa kodu, to konfiguracja mocków. Kod który odpalamy także testuje samą implementacje naszych mocków - przez co gdy zrobimy w niej jakiś błąd to nasz test także się wywali. Dramat.

3. W niektórych przypadkach napisanie testu jest tak skomplikowane, że ludziom po prostu nie chce się tego robić. A to nie jest dobry sygnał, jeżeli zależy nam na jakości.

## Przeciążony kontener IoC
Ostatnim problemem jest przeciążenie kontenera IoC, który tak naprawdę jest niezbędny w naszej aplikacji, żeby zarządzać zależnościami.

1. Stworzenie każdego nowego obiektu wymaga od nas zarejestrowania go w kontenerze. Czyli potrzebujemy jeszcze więcej kodu, który nic nie robi.

2. A jak zapomnimy o jakiejś rejestracji, a nie posiadamy porządnych testów integracyjnych. Błąd zobaczymy dopiero na środowisku, bo nie jest wykrywalny w czasie kompilacji.

# Rozwiązania
Pogadajmy, jak zaadresowaliśmy nasze problemy. Wplątaliśmy w nasz kod programowanie funkcyjne.

## Teoria
Na start trochę teorii. Czym w ogóle jest programowanie funkcyjne? W odróżnieniu od programowania obiektowego - gdzie tworzymy obiekty, które posiadają stan, do którego nie mamy dostępu z zewnątrz i metody, określają zachowanie obiektu, czyli pozwalają nam robić różne rzeczy ze stanem - w programowaniu funkcyjnym stan oraz zachowanie są od siebie oddzielone. 

### Niezmienność
Dodatkowo stan przechowywany w obiektach, nie może zostać zmieniony. Zmiana stanu polega na stworzeniu nowego obiektu na bazie starego i dzieje się to w funkcjach opisujących zachowanie tego obiektu. Przykład mamy poniżej, gdzie kupno wycieczki nie zmienia stanu obiektu Travel, a zwraca nowy obiekt z ustawioną flagą Sold na true.</br>
Jak możemy zaobserwować, to podejście nie jest zbyt wygodne w C#, więc nie stosowaliśmy go zbyt często, zanim nie wprowadzono rekordów. Więc w kodzie funkcja Buy nadal przyjmowała i zwróciłaby obiekt, ale zamiast tworzyć nowego, po prostu zmieniłaby jego stan - związane jest to z problemem rozszerzania obiektu, gdy dojdą nowe pola, musimy pamiętać, żeby dodać je także w każdej funkcji. 

### Czyste funkcje
Zachowanie obiektów trzymamy w czystych funkcjach. Jakie to funkcje? Czysta funkcja to odpowiednik funkcji matematycznej, gdzie jakiemuś wejściu, zawsze odpowiada jakieś wyjście. Nie ma żadnych efektów ubocznych, w postaci operacji IO, zmiany globalnego stanu, czy rzucania wyjątków. Czemu? Bo przez efekty uboczne funkcja staje się nieprzewidywalna. Nie możemy przewidzieć co się stanie w całym systemie jeżeli funkcja zmienia jakiś globalny stan, którego nie znamy, albo co funkcja zwróci, jeżeli nie wiemy co znajduje się w bazie danych.</br>
Ważnym aspektem jest także, że czysta funkcja nie może wywoływać nieczystej, bo znowu nie będziemy mogli przewidzieć co się wydarzy.
        
### Funkcje pierwszoklasowe
Funkcje pierwszoklasowe to tak naprawdę cecha języka programowania, która bardzo ułatwia stosowanie różnych wzorców programowania funkcyjnego. Polega na tym, że język musi wspierać przechowywanie funkcji w zmiennej, przyjmowanie funkcji jako parametr innej funkcji, oraz zwracanie funkcji z funkcji.</br>
Jednym z bardzo użytecznych wzorców funkcyjnych jest Higher Order Function, czyli funkcja, która przyjmuje funkcje w parametrze i zwraca w wyniku inna funkcję. Po prawej mamy przykład użycia HOF jako dekoratora, gdzie funkcja zwracająca obecny czas GetUtcNow została wzbogacona o logowanie.
### Deklaratywność
W gratisie pisząc funkcyjnie otrzymujemy kod deklaratywny. Zamiast skupiać się na tym jak komputer ma przetworzyć nasze dane, skupiamy się na tym, co chcemy uzyskać.</br>
Przykładem kodu deklaratywnego jest kod SQL, gdzie podajemy, że chcemy uzyskać imię i nazwisko użytkowników z Polski, a w jaki sposób serwer to zrobi, to nas już nie interesuje.</br>
LINQ także jest dobrym przykładem kodu deklaratywnego, gdzie zamiast iterować po kolekcji, tworzyć jakieś zmienne pomocnicze, itp. Żeby uzyskać informację, czy użytkownik kupił więcej niż 3 wycieczki w zeszłym roku, po prostu bierzemy listę podróży, filtrujemy te kupione przez użytkownika w zeszłym roku, po czym olewamy pierwsze 3, bo nas nie interesują i sprawdzamy, czy jakiekolwiek jeszcze zostały. Jak można zauważyć kod jest znacząco krótszy, czyta się go łatwiej, bo od góry do dołu i nie musimy pamiętać żadnego stanu podczas debugowania. 

## Praktyka
Dobra koniec tej nudnej teorii, czas na praktykę. Z tego wszystkiego powstało kilka zasad, które stosujemy podczas pisania kodu

### Czyste funkcje gdzie się da
Czyste funkcje gdzie się da, co to znaczy? Próbujemy zmaksymalizować ilość czystych funkcji i przedstawić całą naszą domenę za pomocą nich. Wszystko co nieczyste chcemy przenieść na brzegi aplikacji. W skrócie chcemy uzyskać mniej więcej coś takiego jak na schemacie - przypomina to trochę cebulę, jak w portach i adapterach. Czyli cała nasza domena jest przedstawiona niezmiennymi obiektami, jak Travel, oraz czystymi funkcjami CalculateDiscount…</br>
W drugiej warstwie mamy czyste funkcje, które nie są związane stricte z naszą domeną, a rozwiązują bardziej techniczne problemy, jak mapowanie, czy walidacja requestu. W zewnętrznej warstwie mamy wszystkie nieczyste funkcje, takie jak pobranie obecnego czasu, wyciągnięcie danych z bazy, czy sam handler na request HTTP.</br>
W przykładzie widzimy brzeg aplikacji, gdzie wszystkie nieczyste funkcje są wywoływane z i tak już nieczystego handlera requestu.
        
### Delegaty ponad interfejsy
Każdy interfejs z jedną metodą można zastąpić delegatem do funkcji, jest to dużo wygodniejsze, zależność jest jeszcze lżejsza, bo nie używamy konkretnego interfejsu, a jakiejkolwiek funkcji, no i co najważniejsze w testach możemy w bardzo łatwy sposób zastąpić implementacje bez tworzenia specjalnego fakowego obiektu implementującego dany interfejs - to kolejny kod, którego nie musimy pisać.

### Higher Order Function jest Twoim przyjacielem
Jak już wspominałem, wzorzec HOF jest bardzo przydatny, gdy potrzebujemy stworzyć jakikolwiek dekorator do danej funkcji np. logowanie, zbieranie metryk, cachowanie, tracing, czy retryie.
        
### Unikanie abstrakcji
Zawsze zastanów się 3 razy, zanim stworzysz kolejną abstrakcję. Czym więcej abstrakcji, tym mniej czytelny staje się kod. Ważną, rzeczą jest, żeby używać interfejsu, tylko wtedy gdy jest faktycznie potrzebny, tzn. gdy chcemy zastosować jakiś wzorzec typu dekorator, czy strategia i faktycznie planujemy mieć więcej niż jedna implementację danego interfejsu.</br>
W przykładzie widzimy obiekt, który jest abstrakcją  na bazę danych. Więc tak naprawdę nie mamy dostępu bezpośrednio do samej bazy, przez co, gdy przyjdzie konieczność zmiany mongo na SQL server, po prostu zmienimy sobie implementacje w tej konkretnej klasie. Nie potrzebujemy wcale do tego mieć interfejsu, czyli kolejnej abstrakcji, na abstrakcję na bazę danych. Nie planujemy w ogóle w testach robić nic związanego z bazą danych, więc interfejs tak naprawdę jest nam zupełnie zbędny.
        
### Testowanie
A jak już mowa o testowaniu. Cała domena, oraz funkcje pomocnicze są przetestowane jednostkowo, w bardzo łatwy sposób, widoczny w przykładach. Nie potrzebujemy do tego żadnego mockowania, żadnych dodatkowych bibliotek. Po prostu tworzymy sobie dane wejściowe do testowanej funkcji i sprawdzamy czy zwrócone zostały poprawne dane na wyjściu.</br>
Resztę aplikacji, czyli tak naprawdę logikę znajdującą się na brzegach aplikacji testujemy integracyjnie. Co też nie jest specjalnie trudne, bo tak naprawdę mamy konkretnie określone w funkcjach brzegowych, co dana aplikacja potrzebuje z zewnątrz. Więc wystarczy postawić nasz serwis i bazy danych, wypełnić je danymi i przetestować wszystkie przypadki.</br>
Ewentualnie, jeżeli korzystamy z jakichś trudnych zapytań SQL można zrobić osobne testy dla samych zapytań.

# Podsumowanie
Podsumujmy sobie wszystko - co działa, a co nie.
## Więc, co działa?
        
1. Pierwszą z zalet jest poprawa czytelności kodu. Deklaratywność naprawdę działa cuda. Kod bardziej przypomina angielski, jest łatwiejszy do zrozumienia i łatwiej śledzić w nim co dzieje się z naszym stanem, na którym działamy.
2. Druga sprawa - testy. W całej aplikacji potrzebujemy dużo mniej kodu. Mamy stricte zdefiniowane co musimy przetestować, czego wynikiem jest większe pokrycie testami. No i co najważniejsze, jak trzeba mniej zrobić, to mniej się nie chce.
3. Trzecią rzeczą jest szybszy development. Podczas pisania kodu tworzymy dużo mniej bugów, przez to, że skupiamy się nad tym co chcemy uzyskać, a nie jak to zrobić, ale żeby tego faktycznie doświadczyć to musicie tego sami spróbować. I w ogóle całego kodu jest dużo mniej. Porównując obie implementacje, gdzie specjalnie zostawiłem mniej więcej takie same abstrakcje, kod funkcyjny wraz z testami zajął tylko 481 linii, a obiektowy 580 + dodatkowa biblioteka do tworzenia mocków, która też zaoszczędziła nam sporo linii. 

## Żeby nie było tak kolorowo, co nie działa?

1. Po pierwsze C# nie jest językiem funkcyjnym - stara się, wchodzą coraz to nowsze funkcyjne ficzery, ale nadal brakuje w nim wielu standardowych ficzerów używanych w programowaniu funkcyjnym, przez co musimy tworzyć więcej kodu, niż byłoby to wymagane. Niektórych ficzerów, zważywszy na to jak C# działa w ogóle nigdy nie będzie, wszystkie inne musimy sami napisać, albo skorzystać z zewnętrznych bibliotek.

2. Po drugie jak z wszystkim, co za dużo to niezdrowo - możemy przeholować z funkcyjnością, tzn. Pakować wszystko w onelinery, których nie da się debugować, bo czemu nie? przecież piękny taki krótki kod.</br>
Możemy także przeholować ze zbytnim rozdrabnianiem funkcji, przez co nie unikniemy godzin straconych na debugowaniu, podobnie jak w podejściu obiektowym.</br>
Oraz tworzenie wielu delegatów np do wyciągania danych z bazy, zamiast jednego obiektu. To jest bardziej subiektywne, ponieważ niektóre zespoły własnie tak organizują sobie kod i wcale im to nie przeszkadza.

3. Trzecim problemem, nie takim nagminnym jest wprowadzenie nowego developera. To podejście pisania kodu, nie jest standardowe w C#, przez co nowa osoba ma bardzo wiele rzeczy do nauczenia i na początku, tworzenie przez nią kodu trwa dłużej.

# Plany na przyszłość
Dobra z planów na przyszłość mamy na pewno

## Adaptację rekordów 
Prace nad tym trwają. Uproszczą na pewno sporo kodu, jak i projektowanie domeny.

## Użycie F#
Użycie F#, który jest oparty na dotnecie, a przy okazji posiada wbudowane wszystkie funkcyjne ficzery. Więc można uprościć kod, nie rezygnując przy okazji ze znanych bibliotek.

## Rx .NET
Reaktywne programowanie jest bardzo popularne w językach takich jak Java, Swift, czy JS. A przy okazji także jest oparte na programowaniu funkcyjnym, więc myślę, że warto sprawdzić co i jak.

# Matsy
Dla zainteresowanych podrzucam trochę materiałów. 

## Enrico Buonanno
Po pierwsze książka Enrico Buonanno Functional Programming in C#: How to write better C# code - świetnie opisuje paradygmat funkcyjny z przykładami w C#. Jeżeli chcesz zacząć przygodę z programowaniem funkcyjnym, to uważam, że nie ma nic lepszego.

## Scott Wlaschin
Super jest także cała działalność Scotta Wlaschina, autora strony fsharpforfunandprofit. Znajdują się na niej świetne kursy, jak myśleć funkcyjnie, jak tworzyć dobry kod w F# i mnóstwo filmów z jego prelekcji na temat wszelkiej maści wzorców funkcyjnych.

## Mark Seemann
Kolejną osobą którą warto śledzić jest Mark Seeman, prowadzi bloga, na którego wrzuca sporo swoich przemyśleń i poradników. Dodatkowo polecam jego prelekcje na NDC. Co roku tworzy prezentację, która jest rozszerzeniem poprzedniej.

## Isaac Abraham
Ostatnią osobą którą chcę polecić jest Isaac Abraham, autor książki Get Programming with F#, której przyznam się, nie czytałem, ale kilka osób mi ją bardzo polecało. Oglądałem za to prelekcję Isaaca na meetupie Barcelona .NET Core, w której porównywał C# i F#, którą mogę szczerze polecić.


*** 
images come from twitter pages @ploeh, @ScottWlaschin, @la_yumba, @isaac_abraham</br>
books images come from amazon.com
***