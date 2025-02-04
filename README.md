## DFI CinematekTool
Dette projekt er lavet i forbindelse med hovedopgaven på KEA København og i samarbejde med Det Danske Filminstitut.

Webapplikatioen er en prototype og er vores bud på modernisering af en af DFI's interne webapplikation. 

### Krav:
Programmet er et planlægningsværktøj for tidligere og kommende events/begivenheder afholdt af DFI. En bruger skal kunne have forskellige rettigheder til at oprette en begivendhed med tilhørende film, som vises på til begivenheden, opdatere begivendheden, opdatere film i en begivendhed eller slette en film eller hele begivenheden.

Andre krav:
- Tre brugertyper (Administrator, Programredaktør, Operatør)
- Bruger authentifisering og autorisering
- Sortering af events efter måned / dato
- Administrator skal kunne oprette, opdatere, blokere for brugeradgang og slette brugerer.

Teknologier anvendt:
- C#
- ASP.NET Core
- Entity Framework
- Blazor
- Radzen
- MSSQL
- xUnit til tests

#### Clean Achitecture:
For at holde kodebasen modulerbar, vedligeholdelsesvenlig og testbar, så der anvendt Clean Achitecture til programmets arkitektur. Her brues én solution-fil indeholden class library til henholdsvis Application, Domain og Infrastruture og Blazor til presentation layer. På denne måde opnår vi en højere abstraktion og seperation af bekymringer, samtidig vil hvert lag have sin egen injection-fil til DI-containeren.

![image](https://github.com/user-attachments/assets/8ca03e98-433e-4e95-9eb0-91e58980045d)

![image](https://github.com/user-attachments/assets/0dcc4f9d-34c9-439f-be13-b49ac3c5857d)



#### Login page:
![image](https://github.com/user-attachments/assets/91b6b7d4-8a4f-43d6-90c8-2c6606132650)

#### Main page:
![image](https://github.com/user-attachments/assets/508ccbcb-1807-4bf4-9db7-383dd65d5dd7)

#### Main page (virtualisering slået til):
![image](https://github.com/user-attachments/assets/48866690-b7ca-4e6c-b75c-eaaaf201bb6f)

#### Opret ét event + film:
![image](https://github.com/user-attachments/assets/a0e8fc64-88ce-4ed0-8ba4-6b6ab0aba5a1)
<br><br>
Tilvalg / fravalg af film + søgefunktionalitet
![image](https://github.com/user-attachments/assets/c8c29792-6ede-4e4c-8ac9-31f251e64539)

#### Opdatering og advarelser:
![image](https://github.com/user-attachments/assets/0cea675d-0855-466a-b055-a149865daf58)
<br><br>
![image](https://github.com/user-attachments/assets/e46866fa-7bc8-46e7-aa98-822400ac70c4)

#### Adminstartor / brugerstryring:
![image](https://github.com/user-attachments/assets/1beb83af-ca3a-46a3-bc39-ba97c8e09c85)
![image](https://github.com/user-attachments/assets/34017afc-49a8-43d1-b2ff-df357206aae7)









