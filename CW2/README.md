# apbd-cw2-git-s31540
Project Description

This is a project that manages renting university equipment.
Supports adding users and equipment, renting, returning, checking availability, and generating simple reports.

Design Decisions

Project divided into Domain, Data, Services

Domain contains core objects (Equipment, User, Rental) – only data and simple logic.

Services contains business logic (RentalService) like renting, limits, penalties.

Program handles only user interactions with console and menu.


Cohesion – each class has one job, there is no extra task that are completed by one class.

Low coupling – each class completes its job, indpendently to others. UI, Logic and JSON handling are separate tasks that proccessed by separate entities.

Responsibilities are separated: model, logic, and interface are not mixed.

My project was done this way, that when i was adding changes, i did not have to rewrite everything, instead i only changed one part.
Making changes also was easier because i divided the project by business rules.