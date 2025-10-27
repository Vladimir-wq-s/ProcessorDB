
Установите зависимости (NuGet packages):
Откройте проект в Visual Studio и выполните в Package Manager Console:

Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 9.0.10
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 9.0.10
Install-Package Bogus -Version 35.5.1
Install-Package OxyPlot.Wpf -Version 2.2.0
Или используйте NuGet Manager в VS: найдите и установите пакеты выше.



ProcessorDB - Курсовой проект на .NET Framework
Описание проекта
Это курсовой проект на .NET Framework (C#), реализующий графический интерфейс для управления базой данных процессоров. Проект использует WPF для UI, Entity Framework Core для ORM и SQLite как СУБД. База данных содержит 5 таблиц: Processor (основная сущность), Manufacturer (производители), Country (страны), TechSpec (технические спецификации) и ProductionInfo (информация о производстве).

Ключевые возможности:

Полное CRUD (создание, чтение, обновление, удаление) для всех таблиц.
Поиск и фильтрация процессоров (по названию, цене, производителю).
Отчёты в виде диаграмм (цены и частоты процессоров с использованием OxyPlot).
Асинхронные операции с БД для избежания блокировки UI.
Заполнение БД случайными данными при первом запуске (с помощью Bogus).
Классический WPF-интерфейс с меню, статус-баром, горячими клавишами (Ctrl+N, Ctrl+E, Del) и контекстным меню.
Проект соответствует требованиям: Code First подход в EF Core, не менее 5 таблиц, асинхронность, WPF с темами, поиск/фильтры/отчёты.



Требования к запуску
ОС: Windows 10/11.
IDE: Visual Studio 2022 или новее.
.NET Framework: Версия 8.0 или выше
Дополнительные настройки: Нет специальных (БД создаётся автоматически). При первом запуске миграции применяются, и данные заполняются.



Сборка и запуск:

Откройте ProcessorDB.sln в Visual Studio.
Убедитесь, что Startup Project — ProcessorDB (MainWindow.xaml).
Нажмите F5 или зелёную стрелку. При первом запуске БД создастся и заполнится данными.
Если ошибки: Проверьте строку подключения в AppDbContext.cs (путь к processors.db).
Тестирование:

Добавьте/редактируйте процессоры через меню.
Используйте фильтры (поиск, цена, производитель).
Просмотрите отчёты (диаграммы цен/частот).
Управляйте справочниками (производители, страны).
Структура проекта
Data/AppDbContext.cs: Контекст EF Core, миграции, SeedData.
Models/: Классы-сущности (Processor.cs, etc.).
Migrations/: Автогенерированные миграции EF.
Views/: WPF-окна (MainWindow.xaml, EditProcessorWindow.xaml, etc.).
processors.db: База данных SQLite (создаётся автоматически).
Дополнительные библиотеки и зависимости
Microsoft.EntityFrameworkCore.Sqlite (9.0.10): Для работы с SQLite в EF Core.
Microsoft.EntityFrameworkCore.Tools (9.0.10): Для миграций и Code First.
Bogus (35.5.1): Для генерации фейковых данных при запуске.
OxyPlot.Wpf (2.2.0): Для построения диаграмм в отчётах.
System.ComponentModel.DataAnnotations: Встроенная (для валидации моделей).
Другие: Стандартные WPF-зависимости (.NET Framework).
