# EKupi.King

EKupi King eCommerce

Kreirati asp.net core web api (.net 6)
Koristiti Clean Arhitecture 
	- EKupi.WebApi (.net web api) - Referenca na Application i Domain
	- EKupi.Domain (library) - Nema referenci na druge projekte
	- EKupi.Infrastructure (library) - Referenca na Domain
	- EKupi.Application (library) - Referenca na Infrastructure i Domain
	
Ne koristiti gotov template, napraviti samostalno solution koji uključuje sve projekte.

Folderi u projektima:
EKupi.WebApi - Controllers, Models	
EKupi.Domain - Entities, Enums
EKupi.Infrastructure - Configurations, Migrations, Services
EKupi.Application - Common

Koristiti MediatR pattern za komunikaciju izmedju WebApi i Application
Kontrleri ne smiju sadrzavati nikakvu biznis logiku

Za Autentifikaciju koristiti .net Identity i JWT token
Dodati resourceBasedSecurity koji ograničava korisnike da pregledaju samo naruđžbe koje su oni kreirali

UML diagram baze podataka u prilogu. 
Bazu cu dodatno objasniti, narocito dio sto se tice ProductsRelationship i zasto postoji ta tablica.

Za kreiranje DB koristiti Code First pristup. Svaka relacija u modelu mora biti definirana kroz fluent api.
Kroz sql skriptu koju cemo programski pozvati pri startupu inicijalno dodati podatke u bazu za šifrarnike.
Migracije pokrenuti pri statup-u aplikacije.

Akcije:
AccountController
	- Register
	- Login
	- Logout 
	
ProductsController: 
 - Prikaz proizvoda grupiranih po kategorijama, sa svojim podproizvodima, sorting po cijeni asc i desc
 - Prikaz najprodavanijih proizvoda grupiranih po kategorijama
 - Dodavanje proizvoda i podproizvoda
 - Edit proizvoda i podproizvoda
 - Brisanje (soft delete) 
 
 
 OrdersController
 - CRUD sa orderima - voditi racuna da je proizvod dostupan
 - Ukupna suma novca potrosena na proizvode grupirana po mjesecima
 - top 10 najprodavanijih proizvoda
 - kupci grupirani po potrosnji od najveceg ka najmanjem
 
