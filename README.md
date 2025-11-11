# 🏋️‍♂️ FitnessCenterLab3

ASP.NET Core 8.0 (Minimal API) приложение для лабораторной работы №3 по дисциплине **«Разработка приложений баз данных и систем»**.
[![.NET Build & Test](https://github.com/dmitryspiridonovvv/lab3rbpds/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dmitryspiridonovvv/lab3rbpds/actions/workflows/dotnet.yml)

---

## 📘 Описание

Проект представляет собой **информационную систему фитнес-центра**, содержащую:
- регистрацию клиентов, сотрудников, тренеров и абонементов;
- просмотр таблиц с пагинацией;
- поиск клиентов (через Cookies и Session);
- автоматическую генерацию тестовых данных (1000 клиентов).

---

## ⚙️ Технологии

- **.NET 8 / ASP.NET Core**
- **Entity Framework Core 8**
- **SQL Server LocalDB**
- **C# 12**
- **HTML + CSS (встроенные шаблоны)**

---

## 🗂️ Структура проекта
FitnessCenterLab3/
├── Data/
│ └── ApplicationDbContext.cs
├── Models/
│ ├── Client.cs
│ ├── Employee.cs
│ ├── Trainer.cs
│ ├── Zone.cs
│ ├── MembershipPlan.cs
│ ├── MembershipPlanZone.cs
│ ├── MembershipSale.cs
│ ├── GroupClass.cs
│ ├── ClassSignup.cs
│ └── Visit.cs
├── Services/
│ └── CachedDataService.cs
├── Session/
│ ├── SearchFormState.cs
│ └── SessionExtensions.cs
├── Program.cs
├── appsettings.json
└── README.md

---

## 🚀 Запуск проекта

1. Установи .NET 8 SDK:  
   [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)

2. Клонируй репозиторий:
   ```bash
   git clone https://github.com/dmitryspiridonovvv/lab3rbpds.git
   cd lab3rbpds
🧠 Функционал

/info — главная страница

/table/clients?page=1 — таблица клиентов с пагинацией

/searchform1 — поиск с использованием Cookies

/searchform2 — поиск с использованием Session

🧪 Тестовые данные

При каждом запуске база данных пересоздаётся и заполняется:

1000 клиентов с рандомными именами, полом и телефонами;

зоны (тренажёрный зал, бассейн, групповые занятия).

Дмитрий Спиридонов
Группа: РПБДИС
Лабораторная работа №3 — Fitness Center Management System
