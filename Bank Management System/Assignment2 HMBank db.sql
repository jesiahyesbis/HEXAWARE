
--Assignment 1
create database HMBank;
use HMBank;

--Customers table
create table Customers(
customer_id int primary key identity(1,1),
first_name varchar(50) not null,
last_name varchar(50)not null,
DOB date not null,
email varchar(100) unique not null,
phone_number varchar(15) not null,
address varchar(255) not null 
);



--Accounts table
create table Accounts(
account_id int primary key identity(1,1),
customer_id int not null,
account_type varchar(20) check(account_type in ('savings','current','zero_balance')) not null,
balance decimal(18,2) default 0.00 not null,
constraint FK_Accounts_Customer_Id foreign key(customer_id) references Customers(customer_id) on delete cascade
);





--Transactions table
create table Transactions(
transaction_id int primary key identity(1,1),
account_id int not null,
transaction_type varchar(20) check(transaction_type in ('deposit','withdrawal','transfer')) not null,
amount decimal(18,2) default 0.00 not null,
transaction_date datetime default getdate(),
constraint FK_Transactions_Account_Id foreign key(account_id) references Accounts(account_id) on delete cascade
);






--Assignment 2
--Tasks 2: Select, Where, Between, AND, LIKE:
--1. Insert at least 10 sample records into each of the following tables.  
--* Customers  
--* Accounts
--* Transactions

INSERT INTO Customers (first_name, last_name, DOB, email, phone_number, address)
VALUES 
('Rajesh', 'Sharma', '1985-10-15', 'rajesh@gmail.com', '9876543210', '123 MG Road, Mumbai, Maharashtra'),
('Anjali', 'Kumar', '1990-03-22', 'anjali@gmail.com', '9123456789', '456 Indira Nagar, Bengaluru, Karnataka'),
('Ravi', 'Patel', '1982-07-30', 'ravi@gmail.com', '8234567890', '789 LBS Nagar, New Delhi'),
('Ram', 'Nishanth', '1999-10-05', 'ram@gmail.com', '9765432101', '901 Sector 29, Noida'),
('Ramesh', 'Kumar', '1995-03-28', 'ramesh@gmail.com', '9076543212', '234 Vivekananda Nagar, Chennai'),
('Alice', 'Alcantara', '1972-09-30', 'alice@gmail.com', '7990123456', '567 Model Colony, Pune'),
('Bob', 'Peter', '1981-06-20', 'bob@gmail.com', '9712345678', '890 Sector 17, Chandigarh'),
('Arjun', 'Shibu', '1985-05-16', 'arjun@gmail.com', '9834567890', '345 Defence Colony, New Delhi'),
('Immanuel', 'Joshua', '2004-05-04', 'immanuel@gmail.com', '9743210987', '678 Park Street, Chennai'),
('Anthony', 'Michael', '2006-07-06', 'anthony@gmail.com', ' 9786543210', '567 Swastik Society, Ahmedabad');



INSERT INTO Accounts (customer_id, account_type, balance)
VALUES 
(1, 'savings', 50000.00),
(1, 'current', 20000.00),
(2, 'savings', 0.00),
(3, 'zero_balance', 0.00),
(4, 'current', 25000.00),
(5, 'savings', 18000.00),
(6, 'zero_balance', 0.00),
(7, 'current', 32000.00),
(8, 'savings', 22000.00),
(9, 'zero_balance', 0.00),
(10, 'current', 28000.00);




INSERT INTO Transactions (account_id, transaction_type, amount)
VALUES 
(1, 'deposit', 10000.00),
(1, 'withdrawal', 5000.00),
(2, 'deposit', 15000.00),
(3, 'transfer', 2000.00),
(4, 'deposit', 0.00),
(5, 'deposit', 5000.00),
(6, 'withdrawal', 2000.00),
(7, 'transfer', 1500.00),
(8, 'deposit', 8000.00),
(9, 'withdrawal', 3000.00),
(10, 'transfer', 2500.00),
(11, 'deposit', 10000.00);


select * from Customers;
select * from Accounts;
select * from Transactions;
drop table Customers;
drop table Accounts;
drop table Transactions;

--2. Write SQL queries for the following tasks:
--1. Write a SQL query to retrieve the name, account type and email of all customers.  
SELECT concat(c.first_name,' ', c.last_name) as 'Customer name', a.account_type as 'Account type', c.email as 'Email'
FROM Customers c
JOIN Accounts a ON c.customer_id = a.customer_id;

--2. Write a SQL query to list all transaction corresponding customer.
SELECT concat(c.first_name,' ', c.last_name) as 'Customer name', 
t.transaction_id, t.transaction_type, t.amount,t.transaction_date
FROM Customers c
JOIN Accounts a ON c.customer_id = a.customer_id
JOIN Transactions t ON a.account_id = t.account_id
ORDER BY c.customer_id;


--3. Write a SQL query to increase the balance of a specific account by a certain amount.
UPDATE Accounts
SET balance = balance + 1000.00
WHERE account_id = 2;

select * from Accounts where account_id=2;

--4. Write a SQL query to Combine first and last names of customers as a full_name.
SELECT CONCAT(first_name,' ', last_name) AS 'Full Name'
FROM Customers;

--5. Write a SQL query to remove accounts with a balance of zero where the account
--type is savings.

DELETE FROM Accounts
WHERE balance = 0 and account_type = 'Savings';

Select * from Accounts;

--6. Write a SQL query to Find customers living in a specific city.
select * from Customers where address like('%Chennai%');
select * from Customers where address like('%New Delhi%');

--7. Write a SQL query to Get the account balance for a specific account.
Select account_id,customer_id, balance from Accounts where account_id=4;

Select account_id,customer_id,account_type,balance from Accounts where account_type='Current';

--8. Write a SQL query to List all current accounts with a balance greater than $1,000.
Select * from Accounts where account_type='Current' and balance>1000;

--9. Write a SQL query to Retrieve all transactions for a specific account.
Select * from Transactions where a.account_id=1;

--10. Write a SQL query to Calculate the interest accrued on savings accounts based on a given interest rate. 
--Interest=(balance*number of years*rate of interest)/100 here,number of years 5, rate of interest 2
Select account_id,customer_id,account_type,(balance*5*2)/100 as Interest from Accounts where account_type='Savings';

--11. Write a SQL query to Identify accounts where the balance is less than a specified
--overdraft limit.
Select * from Accounts where balance<30000;

--12. Write a SQL query to Find customers not living in a specific city. 
Select * from Customers where address not like('%New Delhi%');






