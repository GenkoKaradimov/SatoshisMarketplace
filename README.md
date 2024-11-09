# Satoshi's Marketplace
"Satoshi's Marketplace" is a digital marketplace where users can both buy and sell digital products for satoshis (1 Bitcoin is 100 000 000 satoshis). This project was developed as a demo platform to showcase a digital trading environment for educational purposes. Please note that it is not affiliated with any real business and is intended solely as a training tool.

## Features of 1.0.0
The project's core functionalities are outlined below.
 1. User registration, login and logout
 2. User account settings
 3. (TODO) User can set profile picture
 4. Activity logging (e.g., logins, password changes, profile updates)
 5. (TODO) User can deposit and withdraw satoshis to/from the marketplace balance
 6. Administrator Options (create, edit, and manage categories and tags)
 7. (TODO) User can create and publish new products
 8. (TODO) User can his edit existing product details
 9. (TODO) User can release new versions of existing products (add new 'product version')
 10. (TODO) Categorization products by category and tags
 12. (TODO) User can view products by latest releases, specific tags, or categories
 13. (TODO) User can add products to a personal "Favorites" list
 14. (TODO) User can purchase product
 15. (TODO) User can leave reviews on purchased products
 16. (TODO) User can send messages to other user
 17. (TODO) User can transfer satoshis to other users
 18. (TODO) View a complete history of all transactions

 # To Do for next versions
 1. Change password encryption algorithm with more secured one
 2. Filter logs per type and timestamp period (from / to)
 3. Administrator to ban and restrict users
 4. Checking is password good (registration, changing password)
 5. Add email to profile (password recovering)
 6. Admin can configure fees for deposit, withdrawal and sell
 7. Platform to be locked as a store - users connot sell products

## Project Structure
Solution contains 3 projects:
1. **Web** – Handles HTTP requests from users and serves as the main web interface of the project.
2. **Services** - Contains the core business logic, including handling requests to the database and interacting with external services (APIs).
3. **Entities** – Defines the data models and contains migrations for database schema creation and management.

### Database
The database is a Microsoft SQL Server database, created using the 'Code First' approach with Entity Framework Core.
TODO: scheme of database structure

### Services
TODO: list of services and methods

### Web
TODO: description of web service

# Installation
TODO: Dockerize it and add description 

# Configuration 

# Usage

# Contributing
We welcome contributions of all kinds! Here are some ways you can help improve this project: 
1. **Report Bugs** - Found an issue? Please let us know by opening a new issue and providing details. 
2. **Suggest Features** - Have an idea for an improvement? We’d love to hear it! 
3. **Improve Documentation** - Spotted a typo, or feel something could be explained better? Help us improve the docs. 
4. **Develop New Features** - Check out our [Issues](https://github.com/GenkoKaradimov/SatoshisMarketplace/issues) for open feature requests and pick one to work on. 
5. **Optimize Code** - Help make the code more efficient and maintainable. 
6. **Create Tests** - Test cases are always appreciated to help ensure our code works as expected. 
7. **Help with Translations** - If you speak another language, consider helping us translate the project. 

# License
This project is licensed under the **Permissive Open License**.

**Permission is hereby granted to any person obtaining a copy of this project to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the project, without restriction, subject to the following conditions:**

1.  **Attribution** - You must provide attribution to the original author(s) of this project in a reasonable manner, but not in a way that suggests they endorse your modifications or usage.
2.  **No Warranty** - This project is provided "as is," without warranty of any kind, express or implied. The authors are not liable for any damages or issues that arise from using, modifying, or distributing this project.

**By using this project, you accept these terms.**

# Contact
If you have any questions or would like to connect, feel free to reach out via email at genko.karadimov@gmail.com. I’m available for inquiries in both Bulgarian and English.