# WizytowkaFirmy
Funkcjonalności aplikacji 

- wyświetlanie głównych danych firmy,
- wysyłanie wiadomości e-mail po weryfikacji reCAPTCHA,
- udostępnianie lokalizacji przez Google Maps,
- opis  i historia firmy,
- wystawianie opinii przez klientów po weryfikacji reCAPTCHA,
- zarządzanie opiniami przez administratora,
- informacje na temat plików cookies.


Uruchomienie aplikacji

W celu uruchomienia aplikacji należy zmienić ustawienia w pliku appsettings.json. W części “SMTP” należy podać własną konfigurację wysyłania adresu e-mail, zależną
od używanej poczty. Pole “EmailOd” odpowiada adresowi e-mail z jakiego będą przychodzić wiadomości, np. kontakt@firma.pl, pole “Password” przechowuje zaszyfrowane hasło do poczty. Hasło można zaszyfrować przy użyciu aplikacji SzyfrowanieHasel dołączonej na repozytorium. Pole “EmailDo” odpowiada adresowi e-mail, na które mają zostać wysyłane wiadomości od klientów. Dodatkowo w części “ConnectionStrings” należy umieścić connection string pozwalający na połączenie z własną bazą danych.

Obsługa reCAPTCHA

W celu dodania widżetu reCAPTCHA do projektu należy wejść na stronę https://www.google.com/recaptcha, zarejestrować domenę oraz wybrać wersję reCAPTCHA (v2, nie jestem robotem). Po przesłaniu formularza wygenerowane zostaną dwa klucze: publiczny i prywatny. Posiadając te klucze możemy przejść do zabezpieczania formularzy.

W widoku, w którym chcemy używać weryfikacji reCAPTCHA należy dodać:
<script src="https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit" async defer></script>
<script> const siteKey = '@siteKey'; </script>
oraz 
<input type="hidden" id="recaptchaTokenInputId" name="RecaptchaResponse"/>

Aplikacja posiada również serwis odpowiedzialny za weryfikcję reCAPTCHA za pomoca Google API.  
Mechanizm ten umożliwia sprawdzenie, czy użytkownik poprawnie przeszedł test reCAPTCHA, co jest kluczowe dla zabezpieczenia aplikacji przed automatycznymi botami.
Metoda przyjmuje dwa parametry: token reCAPTCHA uzyskany z frontendu oraz klucz tajny powiązany z projektem Google reCAPTCHA. Wysyła następnie żądanie POST do serwera Google, przesyłając oba te parametry. Odpowiedź w formacie JSON jest analizowana, aby określić wynik weryfikacji.
Jeśli weryfikacja zakończy się sukcesem, metoda zwraca pozytywny wynik, który można wykorzystać do dalszego przetwarzania żądań użytkownika. W przypadku błędu lub niepowodzenia weryfikacji zwracana jest wartość negatywna.


Opis commitów

Commit(27-11-2024) - 'Dodaj .gitattributes, .gitignorei README.md' - stworzenie repozytorium

Commit(27-11-2024) - 'Dodaj pliki projektów.' - dodanie plików projektu do repozytorium

Commit(27-11-2024) - 'Commit_SzkieletProjektu' - stworzenie szkieletu projektu, nakreślenie struktury projektu.

Commit(27-11-2024) - 'Update appsettings.json' - aktualizacja pliku konfiguracyjnego json, edycja pól odpowiedzialnych za wysyłanie wiadomości e-mail.

Commit(28-11-2024) - 'Zrobiona strona główna' - stworzenie strony głównej projektu, nakreślenie stylu, kolorów oraz czcionek w projekcie

Commit(28-11-2024) - 'Merge branch master of https://github.com/kkurpiel/WizytowkaFirmy' - zmergowanie gałęzi projektu, która powstała niecelowo

Commit(30-11-2024) - 'Commit_NapiszDoNas_NaszaLokalizacja' - dodanie do projektu widoku pozwalającego na kontakt mailowy oraz widoku udostępniającego lokalizację firmy. Widoków wykorzystujących widżety Google.

Commit(10-12-2024) - 'Uporządkowane widoki i kontrolery.Działające wysyłanie wiadomości e-mail oraz dodawanie opinii. Stworzona baza danych i obsługujący ją serwis.' - zmiana założeń aplikacji (połączenie widoków o-nas oraz nasza-lokalizacja w jeden, ze względu na to, że są to widoki statyczne). Stworzenie prostej bazy danych z wykorzystaniem EntityFramework, składającej się z trzech tabel: Klient, OpiniaKlienta oraz WiadomoscEmail. Stworzenie serwisu DbService obsługującego zapytania do bazy danych.

Commit(17-12-2024) - 'Dodanie panelu administratora do zarządzania wystawionymi opiniami.' - wprowadzenie do aplikacji tokenu uwierzytelniającego JWT, który pozwala na wygenerowanie linku dla administratora dającego możliwość ukrywania opinii oraz wygasającego po 15 minutach.

Commit(21.01.2025) - 'Końcowa wersja aplikacji WizytowkaFirmy' - dodanie komunikatu o plikach Cookies oraz widoku odpowiedzialnego za wyświetlanie szczegółów dotyczących polityki prywatności.

Commit(21-01-2025) - 'Uzupełnienie README.md' - uzupełnienie informacji o zmianach aplikcji w poszczególnych commitach.
