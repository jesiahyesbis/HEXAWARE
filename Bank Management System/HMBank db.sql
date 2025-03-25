--Assignment1
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

INSERT INTO Customers (first_name, last_name, DOB, email, phone_number, address)
VALUES 
('Rajesh', 'Sharma', '1985-10-15', 'rajesh@gmail.com', '9876543210', '123 MG Road, Mumbai, Maharashtra'),
('Anjali', 'Kumar', '1990-03-22', 'anjali@gmail.com', '9123456789', '456 Indira Nagar, Bengaluru, Karnataka'),
('Ravi', 'Patel', '1982-07-30', 'ravi.@gmail.com', '8234567890', '789 LBS Nagar, Delhi');


--Accounts table
create table Accounts(
account_id int primary key identity(1,1),
customer_id int not null,
account_type varchar(20) check(account_type in ('savings','current','zero_balance')) not null,
balance decimal(18,2) default 0.00 not null,
constraint FK_Accounts_Customer_Id foreign key(customer_id) references Customers(customer_id) on delete cascade
);

INSERT INTO Accounts (customer_id, account_type, balance)
VALUES 
(1, 'savings', 50000.00),
(1, 'current', 20000.00),
(2, 'savings', 30000.00),
(3, 'zero_balance', 0.00);



--Transactions table
create table Transactions(
transaction_id int primary key identity(1,1),
account_id int not null,
transaction_type varchar(20) check(transaction_type in ('deposit','withdrawal','transfer')) not null,
amount decimal(18,2) default 0.00 not null,
transaction_date datetime default getdate(),
constraint FK_Transactions_Account_Id foreign key(account_id) references Accounts(account_id) on delete cascade
);



INSERT INTO Transactions (account_id, transaction_type, amount)
VALUES 
(1, 'deposit', 10000.00),
(1, 'withdrawal', 5000.00),
(2, 'deposit', 15000.00),
(3, 'transfer', 2000.00),
(4, 'deposit', 0.00);

select * from Customers;
select * from Accounts;
select * from Transactions;
