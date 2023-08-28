# Тестовое задание 
## _ПИК-Digital. Отдел разработки и автоматизации_

[Екатерина Швец](https://hh.ru/resume/ad1e76b0ff0791e4f30039ed1f73316641356c)

[Ссылка на задание](https://docs.google.com/document/d/1BxLjxJ8EjEf4h8tkxenMCDTmZabrFLzKARBNnnTloF0/edit)

Решение состоит из 2-ух проектов:
- RibbonBuilder 
- Feature

## RibbonBuilder
Этот проект предназначен для создания в Revit панели с кнопкой, по нажатию на которую будет запускаться приложение.
Проект содержит единственный класс RibbonBuilder, который реализует интерфейс IExternalApplication.
В этом проекте также находится addin-файл _BIMToolKit.addin_ с информацией, необходимой для регистрации плагина в Revit.
## Feature
Это основной проект с решением тестового задания. Представляет собой WPF-приложение по технологии MVVM (для реализации MVVM применялся Prism). Его основные части:
- Класс _Cmd_ - класс команды. Реализует интерфейс IExternalCommand
- Класс _Config_ - класс, конфигурирующий DI-контейнер (использовался контейнер Microsoft.Extensions.DependencyInjection)
- Папка _Views_ содержит представление - UI-окно с кнопкой, полем для вывода информации и статус-баром
- Папка _ViewModels_ содержит viewModel для взаимосдействия с окном
- Папка _Models_ содержит необходимые для работы анемические модели
- Папка _Abstractions_ содержит интерфейсы
- В папке _Services_ находятся классы-сервисы, в них сосредоточена основная логика приложения. Главный из сервисов - ✨ _BusinessLogic_ ✨

## Установка
- Для проверки плагина необходимо через AddInManager загрузить приложение (проект RibbonBuilder), либо только команду (проект Feature) и протестировать ее.

Надеюсь, на вашем компьютере все запустится
