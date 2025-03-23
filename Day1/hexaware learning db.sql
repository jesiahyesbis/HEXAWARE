create database hexaware;
use hexaware;
create table author(author_id int,author_name varchar(20));
insert into author values(101,'Jesiah');
insert into author values(102,'Ram');
insert into author values(102,'Anthony');
insert into author values(103,'Yesbi');
insert into author values(104,'Nishanth');
insert into author values(103,'Yesbi');
insert into author values(105,'Michael');
select * from author;
create table student(student_id int, stu_name varchar(20));
insert into student values(105, 'Michael');
insert into student values(101, 'Jesiah');
insert into student(student_id) values(109);
insert into student(stu_name) values('Ramu')

--error query nullable 
Alter table student add constraint STUDENT_STU_ID Primary KEY(student_id);
select * from INFORMATION_SCHEMA.COLUMNS where table_name='student';
Alter table student alter column student_id int not null;
delete from student where stu_name='Ramu';
select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where table_name='student';
--error queries end

drop table student;
drop table author;

-- the below statements should throw error because tables are deleted
--select * from student;
--select * from author;
--(error message)Msg 208, Level 16, State 1, Line 30
--Invalid object name 'student'.
--Msg 208, Level 16, State 1, Line 31
--Invalid object name 'author'.

select * from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where table_name='author';

--Query with primary key
create table author(author_id int primary key, author_name varchar(20));
insert into author values(101,'Jesi');
insert into author values(102,'Jes');
insert into author values(103,'Yesbi');
insert into author values(104,'Jesiah');
insert into author values(105,'Jasper');
--error query duplicate value
insert into author values(103,'Yesbi');
--error message=Violation of PRIMARY KEY constraint 'PK__author__86516BCF7E3D1A33'. Cannot insert duplicate key in object 'dbo.author'. The duplicate key value is (103).
drop table author;



--CONSTRAINTS

--placement of constraint near the variable
create table author(author_id int constraint PK_Author_ID Primary key, author_name varchar(20));
drop table author;
--placement of constraint at the end before closing bracket and specifying the column name in bracket
create table author(author_id int, author_name varchar(20),constraint PK_Author_ID primary key(author_id));

--ALTER COMMANDS
create table employee(emp_id int ,emp_name varchar(20),salary decimal(8,2));
--add new column email in table employee
Alter table employee add email varchar(255) unique;
--modify column size from 255 to 500 changed
Alter table employee Alter column email varchar(500);

insert into employee values(101,'Alice',3400,'alice@gmail.com');
--error query duplicate email
insert into employee values(102, 'Bob', 3400, 'alice@gmail.com');

--drop constraint
Alter table employee drop constraint UQ__employee__AB6E6164CCAD4603;
select * from employee

--Add constraint
Alter table employee add constraint UQ_Employee_Email unique(email);
--update employee set email='bob@gmail.com' where emp_id=102;
select * from employee

--rename column
exec sp_rename 'employee.email', 'email_address','column';

insert into author values(101,'Richard');
insert into author values(102,'Joe');
insert into author values(103,'Emma');
insert into author values(104,'Jona');


create table book(book_id int constraint PK_Book_ID Primary key,book_name varchar(30),
price numeric(10,2) constraint CK_Price Check(price>=1000),
auth_id int constraint FK_Book_AuthID Foreign key references Author(author_id));

insert into book values(1,'Alice in wonderland',1800,101);
insert into book values(2,'Little red riding hood',3800,101);
insert into book values(3,'Ripvan winkle',3800,104);
insert into book values(4,'Hansel and gratel',3800,103);
delete from author where author_id=102;
delete from author where author_id=103;


select * from author;


--on delete no action
Alter table book drop constraint FK_Book_AuthID;
Alter table book add constraint FK_Book_AuthID 
Foreign key references Author(author_id) On DELETE NO Action;

delete from author where author_id=103;
select * from author;
select * from book;
drop table book;


--on delete set null
create table book(book_id int constraint PK_Book_id Primary Key,
book_name varchar(30), 
price numeric(10,2) constraint CK_Price Check(price>=1000), 
auth_id int constraint FK_Book_Auth1  Foreign Key references Author(author_id) on delete set null) ;

insert into book values(1,'Alice in wonderland',1800,101);
insert into book values(2,'Little red riding hood',3800,101);
insert into book values(3,'Ripvan winkle',3800,104);
insert into book values(4,'Hansel and gratel',3800,103);

delete from author where author_id=103
select * from book;


alter table book drop constraint FK_Book_Auth1;

--on delete cascade
Alter table book add constraint FK_Book_Auth2 FOREIGN KEY(author_id) references Author(author_id) on delete cascade;
delete from author where author_id=101;

--default
create table person(id int,pname varchar(10),prole varchar(20) default 'Guest');
insert into person values(1,'Alice','Admin');
insert into person(id,pname) values (2,'Bob');
select * from person;

--on delete set default
create table departments(deptId int primary Key,dname varchar(10))

create table employees(eid int primary key,ename varchar(20),dept_id int default 1, 
constraint FK_EMP_DID foreign key(dept_id) references departments(deptId) on delete set default);
insert into employees values ( 101 , ' Alice ' , 1)
insert into employees values ( 102 , ' Alice ' , 2)
insert into employees values ( 103 , ' Alice ' , 3)

select * from employees;
delete from departments where deptId=2;
select * from departments;

--change db name
create database demo1;
alter database demo1 MODIFY NAME = new_demo;
drop database new_demo;
truncate table employees;
