
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
transaction_date date,
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




INSERT INTO Transactions (account_id, transaction_type, amount,transaction_date)
VALUES 
(1, 'deposit', 10000.00,'2020-02-12'),
(1, 'withdrawal', 5000.00,'2022-01-05'),
(2, 'deposit', 15000.00,'2021-06-20'),
(3, 'transfer', 2000.00,'2023-03-25'),
(4, 'deposit', 0.00,'2022-05-15'),
(5, 'deposit', 5000.00,'2020-10-05'),
(6, 'withdrawal', 2000.00,'2024-07-02'),
(7, 'transfer', 1500.00,'2022-09-22'),
(8, 'deposit', 8000.00,'2025-01-01'),
(9, 'withdrawal', 3000.00,'2022-11-18'),
(10, 'transfer', 2500.00,'2023-08-10'),
(11, 'deposit', 10000.00,'2022-12-31');


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



--Assignment 3
--Tasks 3: Aggregate functions->avg,sum,min,max, Having, Order By, GroupBy and Joins:
--1. Write a SQL query to Find the average account balance for all customers. 
Select avg(balance) as 'Average Balance' from Accounts;

--2. Write a SQL query to Retrieve the top 10 highest account balances.  
Select top 10 balance,account_id,customer_id from Accounts order by balance desc;

--3. Write a SQL query to Calculate Total Deposits for All Customers in specific date.
SELECT SUM(amount) AS 'Total Deposits'
FROM Transactions WHERE transaction_type = 'deposit'and transaction_date = '2020-02-12';


--4. Write a SQL query to Find the Oldest and Newest Customers.
--Oldest Customer
Select Top 1 * FROM Customers order by DOB asc;
--Newest Customer
Select Top 1 * FROM Customers order by DOB desc;

--5. Write a SQL query to Retrieve transaction details along with the account type.
Select t.*,a.account_type from Transactions t join Accounts a on t.account_id=a.account_id;

--6. Write a SQL query to Get a list of customers along with their account details.
Select c.*,a.account_id,a.account_type,a.balance from Customers c join Accounts a on c.customer_id=a.customer_id;

--7. Write a SQL query to Retrieve transaction details along with customer information for a specific account.
Select  concat(c.first_name, ' ',c.last_name) as 'Name', t.* from Transactions t 
join Accounts a on t.account_id=a.account_id 
join Customers c on a.customer_id=c.customer_id WHERE t.account_id = 2; 

--8. Write a SQL query to Identify customers who have more than one account.
Select concat(c.first_name, ' ',c.last_name) as 'Name' ,c.customer_id,
count(a.account_id) as 'No of Accounts'
from Customers c join Accounts a 
on a.customer_id=c.customer_id  
group by c.customer_id,c.first_name,c.last_name
having count(a.account_id)>1;

--9. Write a SQL query to Calculate the difference in transaction amounts between deposits and withdrawals.
SELECT 
    account_id,
    SUM(CASE WHEN transaction_type = 'deposit' THEN amount ELSE 0 END) 
	- 
    SUM(CASE WHEN transaction_type = 'withdrawal' THEN amount ELSE 0 END) AS 'Difference'
FROM Transactions
GROUP BY account_id;

--10. Write a SQL query to Calculate the average daily balance for each account over a specified period.
SELECT 
    account_id,
    AVG(amount) AS 'Average daily balance'
FROM Transactions
WHERE transaction_date BETWEEN '2020-02-12' AND '2025-01-01'
GROUP BY account_id;

--11. Calculate the total balance for each account type.
Select account_type,sum(balance) as 'Total Balance' from Accounts group by account_type;

--12. Identify accounts with the highest number of transactions order by descending order.
Select account_id,count(*) as Transaction_count from Transactions  
group by account_id order by Transaction_count desc;

--13. List customers with high aggregate account balances, along with their account types.
SELECT 
    c.customer_id,
    c.first_name,
    c.last_name,
    a.account_type,
    SUM(a.balance) AS aggregate_balance
FROM Customers c
JOIN Accounts a ON c.customer_id = a.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name, a.account_type
HAVING 
    SUM(a.balance) > 20000
ORDER BY 
    aggregate_balance DESC;


--14. Identify and list duplicate transactions based on transaction amount, date, and account.
INSERT INTO Transactions (account_id, transaction_type, amount, transaction_date)
VALUES 
(1, 'deposit', 10000.00, '2020-02-12'),
(1, 'deposit', 10000.00, '2020-02-12'),
(2, 'deposit', 15000.00, '2021-06-20'),
(2, 'deposit', 15000.00, '2021-06-20'),
(8, 'deposit', 8000.00, '2025-01-01');

select account_id, amount, transaction_date, count(*) AS duplicate_count
from Transactions
group by  account_id, amount,transaction_date
having COUNT(*) > 1;

--Tasks 4: Subquery and its type:
--1. Retrieve the customer(s) with the highest account balance.
Select c.*,a.balance from Customers c join Accounts a on a.customer_id=c.customer_id
where a.balance=(Select max(balance) from Accounts);

--2. Calculate the average account balance for customers who have more than one account.
select avg(avg_balance) as average_balance from (
    SELECT c.customer_id, AVG(a.balance) AS avg_balance
    FROM Customers c
    JOIN Accounts a ON c.customer_id = a.customer_id
    GROUP BY c.customer_id
    HAVING COUNT(a.account_id) > 1
) AS customers_many_accounts;

--3. Retrieve accounts with transactions whose amounts exceed the average transaction amount.
select distinct a.* from Accounts a
join Transactions t on a.account_id = t.account_id
where t.amount > (select avg(amount) from Transactions);

--4. Identify customers who have no recorded transactions.
INSERT INTO Customers (first_name, last_name, DOB, email, phone_number, address)
VALUES 
('Henry', 'Cavial', '1984-10-15', 'henry@gmail.com', '9000543210', '69 AB Road, Crownpet, Chennai');

INSERT INTO Accounts (customer_id, account_type, balance)
VALUES 
(11, 'current', 0.00);


select c.* from Customers c where c.customer_id not in (
 select DISTINCT a.customer_id
 from Accounts a join Transactions t on a.account_id = t.account_id
);

--5. Calculate the total balance of accounts with no recorded transactions.
select sum(balance) as total_balance
from Accounts where account_id NOT IN (select DISTINCT account_id from Transactions);

--6. Retrieve transactions for accounts with the lowest balance.
Select t.* from Transactions t where 
t.account_id in (select account_id  from Accounts 
where balance=(Select min(balance) from Accounts));

--7. Identify customers who have accounts of multiple types.
select c.* from Customers c where c.customer_id in(Select customer_id 
from Accounts 
group by customer_id
having count(DISTINCT account_type)>1);


--8. Calculate the percentage of each account type out of the total number of accounts.
select account_type, count(*) as Count, count(*) * 100.0 / (select count(*) from Accounts) as Percentage
from Accounts
group by account_type;


--9. Retrieve all transactions for a customer with a given customer_id.
select t.* from Transactions t where t.account_id in (
select account_id from Accounts where customer_id = 2
);

--10. Calculate the total balance for each account type, including a subquery within the SELECT clause.
select a.account_type,
(select sum(balance) from Accounts where 
 account_type = a.account_type
 ) as total_balance
from Accounts a
group by a.account_type;
