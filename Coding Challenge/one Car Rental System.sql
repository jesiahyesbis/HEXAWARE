create database Coding_Challenge_Car_Rental_System;
use Coding_Challenge_Car_Rental_System;

create table Vehicle (
    vehicleID int constraint PK_Vehicle_ID primary key identity(1,1),
    make varchar(50) not null,
    model varchar(50) not null,
    year int not null,
    dailyRate decimal(10,2) not null,
    status bit not null,
    passengerCapacity int not null,
    engineCapacity decimal(7,2) not null 
);
create table Customer (
    customerID int constraint PK_Customer_ID primary key identity(1,1),
    firstName varchar(50) not null,
    lastName varchar(50) not null,
    email varchar(100) constraint U_C_Email unique not null,
    phoneNumber varchar(20) constraint U_C_Phone not null
);

create table Lease (
    leaseID int constraint PK_Lease_Id primary key identity(1,1),
    vehicleID int not null,
    customerID int not null,
    startDate date not null,
    endDate date not null,
    type varchar(20) constraint CK_L_type check (type in ('DailyLease', 'MonthlyLease')) not null,
    constraint FK_Vehicle_Id foreign key(vehicleID) references Vehicle(vehicleID),
	constraint FK_Customer_Id foreign key(customerID) references Customer(customerID)
);

create table Payment (
    paymentID int constraint PK_Payment_Id primary key identity(1,1),
    leaseID int not null,
    paymentDate date not null,
    amount decimal(10,2) not null,
    constraint FK_Lease_Id foreign key(leaseID) references Lease(leaseID)
);

insert into Vehicle (make, model, year, dailyRate, status, passengerCapacity, engineCapacity)  
values  
    ('Toyota', 'Camry', 2022, 50.00, 1, 4, 1450),  
    ('Honda', 'Civic', 2023, 45.00, 1, 7, 1500),  
    ('Ford', 'Focus', 2022, 48.00, 0, 4, 1400),  
    ('Nissan', 'Altima', 2023, 52.00, 1, 7, 1200),  
    ('Chevrolet', 'Malibu', 2022, 47.00, 1, 4, 1800),  
    ('Hyundai', 'Sonata', 2023, 49.00, 0, 7, 1400),  
    ('BMW', '3 Series', 2023, 60.00, 1, 7, 2499),  
    ('Mercedes', 'C-Class', 2022, 58.00, 1, 8, 2599),  
    ('Audi', 'A4', 2022, 55.00, 0, 4, 2500),  
    ('Lexus', 'ES', 2023, 54.00, 1, 4, 2500);

insert Customer (firstName, lastName, email, phoneNumber)  
values  
    ('John', 'Doe', 'johndoe@example.com', '555-555-5555'),  
    ('Jane', 'Smith', 'janesmith@example.com', '555-123-4567'),  
    ('Robert', 'Johnson', 'robert@example.com', '555-789-1234'),  
    ('Sarah', 'Brown', 'sarah@example.com', '555-456-7890'),  
    ('David', 'Lee', 'david@example.com', '555-987-6543'),  
    ('Laura', 'Hall', 'laura@example.com', '555-234-5678'),  
    ('Michael', 'Davis', 'michael@example.com', '555-876-5432'),  
    ('Emma', 'Wilson', 'emma@example.com', '555-432-1098'),  
    ('William', 'Taylor', 'william@example.com', '555-321-6547'),  
    ('Olivia', 'Adams', 'olivia@example.com', '555-765-4321');

insert into Lease (vehicleID, customerID, startDate, endDate, type)  
values  
    (1, 1, '2023-01-01', '2023-01-05', 'DailyLease'),  
    (2, 2, '2023-02-15', '2023-02-28', 'MonthlyLease'),  
    (3, 3, '2023-03-10', '2023-03-15', 'DailyLease'),  
    (4, 4, '2023-04-20', '2023-04-30', 'MonthlyLease'),  
    (5, 5, '2023-05-05', '2023-05-10', 'DailyLease'),  
    (4, 3, '2023-06-15', '2023-06-30', 'MonthlyLease'),  
    (7, 7, '2023-07-01', '2023-07-10', 'DailyLease'),  
    (8, 8, '2023-08-12', '2023-08-15', 'MonthlyLease'),  
    (3, 3, '2023-09-07', '2023-09-10', 'DailyLease'),  
    (10, 10, '2023-10-10', '2023-10-31', 'MonthlyLease');

insert into Payment (leaseID, paymentDate, amount)  
values  
    (1, '2023-01-03', 200.00),  
    (2, '2023-02-20', 1000.00),  
    (3, '2023-03-12', 75.00),  
    (4, '2023-04-25', 900.00),  
    (5, '2023-05-07', 60.00),  
    (6, '2023-06-18', 1200.00),  
    (7, '2023-07-03', 40.00),  
    (8, '2023-08-14', 1100.00),  
    (9, '2023-09-09', 80.00),  
    (10, '2023-10-25', 1500.00);


--1. Update the daily rate for a Mercedes car to 68.
update Vehicle set dailyRate=68 where make='Mercedes';

--2. Delete a specific customer and all associated leases and payments.
declare @custID int = (select customerID from Customer where email='olivia@example.com')
delete from Payment where leaseID in (select leaseID from Lease where customerID=@custID)
delete from Lease where customerID=@custID
delete from Customer where customerID=@custID

--3. Rename the "paymentDate" column in the Payment table to "transactionDate".
exec sp_rename 'Payment.paymentDate','transactionDate','column';

--4. Find a specific customer by email.
select * from Customer where email='johndoe@example.com';

--5. Get active leases for a specific customer.
select * from Lease where customerID=3;
--or
select * from Lease where endDate>getdate() and customerID=2;

--6. Find all payments made by a customer with a specific phone number.
Select p.*,l.customerID,concat(c.firstName,' ',c.lastName) as Customer_Name from Payment p 
left join Lease l on l.leaseID=p.leaseID left join Customer c on 
l.customerID=c.customerID
where c.phoneNumber='555-987-6543';

--7. Calculate the average daily rate of all available cars.
Select avg(dailyRate) as 'Avg Daily Rate' from Vehicle where status=1;

--8. Find the car with the highest daily rate.
Select top 1 * from Vehicle order by dailyRate desc;

--9. Retrieve all cars leased by a specific customer.

select v.* from Vehicle v
join Lease l on v.vehicleID=l.vehicleID
join Customer c on l.customerID=c.customerID
where c.customerID=7;

--10. Find the details of the most recent lease.
select top 1 * from Lease order by startDate desc;

--11. List all payments made in the year 2023.
Select * from Payment where year(transactionDate)=2023;

--12. Retrieve customers who have not made any payments.
select * from Customer where customerID 
not in (select distinct customerID from Lease where vehicleID in (select vehicleID from Payment));

--13. Retrieve Car Details and Their Total Payments.
select v.*,sum(p.amount) as TotalPayments from Vehicle v
join Lease l on v.vehicleID=l.vehicleID
join Payment p on l.leaseID=p.leaseID
group by v.vehicleID,v.make,v.model,v.dailyRate,v.status,v.year,v.passengerCapacity,v.engineCapacity;

--14. Calculate Total Payments for Each Customer.
select c.*,sum(p.amount) as TotalPayments from Customer c
join Lease l on c.customerID=l.customerID
join Payment p on l.leaseID=p.leaseID
group by c.customerID,c.firstName,c.lastName,c.email,c.phoneNumber;

--15. List Car Details for Each Lease.
select l.leaseID as LeaseID,v.* from Lease l
join Vehicle v on l.vehicleID=v.vehicleID;

--16. Retrieve Details of Active Leases with Customer and Car Information.
insert into Lease (vehicleID, customerID, startDate, endDate, type)  
values  
    (1, 2, '2025-03-01', '2025-03-28', 'MonthlyLease'); 

select l.*,c.firstName,c.lastName,c.email,c.phoneNumber,v.make,v.model,v.dailyRate,v.status  
from Lease l  
join Customer c on l.customerID = c.customerID  
join Vehicle v on l.vehicleID = v.vehicleID  
where l.endDate > getdate();

--17. Find the Customer Who Has Spent the Most on Leases.
Select c.*,p.amount from Customer c join Lease l on c.customerID=l.customerID
join Payment p on l.leaseID=p.leaseID
where p.amount in (select max(amount) from Payment);

--18. List All Cars with Their Current Lease Information.
select v.*,l.* from Vehicle v
left join Lease l on v.vehicleID=l.vehicleID
where l.endDate>getdate();

	select * from Vehicle;
	select * from Customer;
	select * from Lease;
	select * from Payment;