# ProjektZaawansowaneProgramowanie





# \# QuizSystem – Projekt z Programowania Zaawansowanego



## \## Opis projektu



QuizSystem to desktopowa aplikacja edukacyjna typu quiz, wykonana w technologii .NET (WPF).

Celem projektu jest demonstracja zaawansowanych zagadnień programowania obiektowego,

architektury warstwowej oraz wzorca MVVM w praktycznym zastosowaniu.



Aplikacja umożliwia użytkownikowi wybór quizu z listy, rozwiązywanie pytań jednokrotnego

i wielokrotnego wyboru, automatyczne sprawdzanie odpowiedzi oraz prezentację wyniku

końcowego wraz ze szczegółowym podsumowaniem.



Projekt został zrealizowany w ramach przedmiotu \*\*Programowanie Zaawansowane\*\*.



---



\## Użytkownicy

\- studenci i uczniowie

\- osoby uczące się podstaw platformy .NET oraz języka C#



---



\## Platforma

\- Windows 10 / 11

\- aplikacja desktopowa (WPF)



---



\## Funkcjonalności

\- wyświetlanie listy dostępnych quizów

\- filtrowanie quizów po tytule i opisie

\- rozpoczęcie i restart quizu

\- nawigacja pomiędzy pytaniami

\- obsługa pytań:

&nbsp; - jednokrotnego wyboru

&nbsp; - wielokrotnego wyboru

\- zaznaczanie odpowiedzi przez użytkownika

\- automatyczne sprawdzanie poprawności odpowiedzi

\- obliczanie wyniku końcowego

\- podsumowanie quizu z wizualnym oznaczeniem odpowiedzi:

&nbsp; - poprawne – kolor zielony

&nbsp; - błędne – kolor czerwony

\- zakończenie quizu i wyjście z aplikacji



---



\## Architektura systemu

Projekt wykorzystuje architekturę warstwową zgodną z zasadami Clean Architecture

oraz wzorzec MVVM.



\### Warstwy:

\- \*\*QuizSystem.Core\*\*  

&nbsp; Logika domenowa, encje (Quiz, Question, Answer), interfejsy oraz reguły biznesowe.

\- \*\*QuizSystem.Infrastructure\*\*  

&nbsp; Dostęp do danych z wykorzystaniem Entity Framework Core, SQLite, migracje i seed danych.

\- \*\*QuizSystem.Wpf\*\*  

&nbsp; Warstwa prezentacji (WPF), ViewModels, bindingi XAML, brak logiki biznesowej w View.



Komunikacja pomiędzy warstwami odbywa się wyłącznie przez abstrakcje (interfejsy).



---



\## Technologie

\- C#

\- .NET 7 / 8

\- WPF

\- MVVM

\- Entity Framework Core

\- SQLite

\- LINQ

\- XAML (Styles, Triggers)

\- Repository Pattern

\- Git / GitHub



---



\## Uruchomienie aplikacji

Gotowa wersja aplikacji znajduje się w katalogu:.\\Release16012026





