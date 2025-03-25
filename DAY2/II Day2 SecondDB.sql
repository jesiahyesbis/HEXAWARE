create database SecondDB;
use SecondDB;

CREATE TABLE regions (
	region_id INT  PRIMARY KEY,
	region_name VARCHAR (25) DEFAULT NULL
);

CREATE TABLE countries (
	country_id CHAR (2) PRIMARY KEY,
	country_name VARCHAR (40) DEFAULT NULL,
	region_id INT NOT NULL,
	FOREIGN KEY (region_id) REFERENCES regions (region_id) ON DELETE CASCADE ON UPDATE CASCADE
);
 
 
 CREATE TABLE locations (
	location_id INT  PRIMARY KEY,
	street_address VARCHAR (40) DEFAULT NULL,
	postal_code VARCHAR (12) DEFAULT NULL,
	city VARCHAR (30) NOT NULL,
	state_province VARCHAR (25) DEFAULT NULL,
	country_id CHAR (2) NOT NULL,
	FOREIGN KEY (country_id) REFERENCES countries (country_id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE jobs (
	job_id INT  PRIMARY KEY,
	job_title VARCHAR (35) NOT NULL,
	min_salary DECIMAL (8, 2) DEFAULT NULL,
	max_salary DECIMAL (8, 2) DEFAULT NULL
);
 
 CREATE TABLE departments (
	department_id INT  PRIMARY KEY,
	department_name VARCHAR (30) NOT NULL,
	location_id INT DEFAULT NULL,
	FOREIGN KEY (location_id) REFERENCES locations (location_id) ON DELETE CASCADE ON UPDATE CASCADE
);
CREATE TABLE employees (
	employee_id INT  PRIMARY KEY,
	first_name VARCHAR (20) DEFAULT NULL,
	last_name VARCHAR (25) NOT NULL,
	email VARCHAR (100) NOT NULL,
	phone_number VARCHAR (20) DEFAULT NULL,
	hire_date DATE NOT NULL,
	job_id INT NOT NULL,
	salary DECIMAL (8, 2) NOT NULL,
	manager_id INT DEFAULT NULL,
	department_id INT DEFAULT NULL,
	FOREIGN KEY (job_id) REFERENCES jobs (job_id) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (department_id) REFERENCES departments (department_id) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (manager_id) REFERENCES employees (employee_id)
);

CREATE TABLE dependents (
	dependent_id INT  PRIMARY KEY,
	first_name VARCHAR (50) NOT NULL,
	last_name VARCHAR (50) NOT NULL,
	relationship VARCHAR (25) NOT NULL,
	employee_id INT NOT NULL,
	FOREIGN KEY (employee_id) REFERENCES employees (employee_id) ON DELETE CASCADE ON UPDATE CASCADE
);


INSERT INTO regions(region_id,region_name) VALUES (1,'Europe');
INSERT INTO regions(region_id,region_name) VALUES (2,'Americas');
INSERT INTO regions(region_id,region_name) VALUES (3,'Asia');
INSERT INTO regions(region_id,region_name) VALUES (4,'Middle East and Africa');

INSERT INTO countries(country_id,country_name,region_id) VALUES ('AR','Argentina',2);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('AU','Australia',3);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('BE','Belgium',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('BR','Brazil',2);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('CA','Canada',2);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('CH','Switzerland',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('CN','China',3);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('DE','Germany',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('DK','Denmark',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('EG','Egypt',4);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('FR','France',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('HK','HongKong',3);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('IL','Israel',4);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('IN','India',3);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('IT','Italy',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('JP','Japan',3);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('KW','Kuwait',4);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('MX','Mexico',2);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('NG','Nigeria',4);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('NL','Netherlands',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('SG','Singapore',3);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('UK','United Kingdom',1);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('US','United States of America',2);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('ZM','Zambia',4);
INSERT INTO countries(country_id,country_name,region_id) VALUES ('ZW','Zimbabwe',4);
 
INSERT INTO locations(location_id,street_address,postal_code,city,state_province,country_id) VALUES (1400,'2014 Jabberwocky Rd','26192','Southlake','Texas','US');
INSERT INTO locations(location_id,street_address,postal_code,city,state_province,country_id) VALUES (1500,'2011 Interiors Blvd','99236','South San Francisco','California','US');
INSERT INTO locations(location_id,street_address,postal_code,city,state_province,country_id) VALUES (1700,'2004 Charade Rd','98199','Seattle','Washington','US');
INSERT INTO locations(location_id,street_address,postal_code,city,state_province,country_id) VALUES (1800,'147 Spadina Ave','M5V 2L7','Toronto','Ontario','CA');
INSERT INTO locations(location_id,street_address,postal_code,city,state_province,country_id) VALUES (2400,'8204 Arthur St',NULL,'London',NULL,'UK');
INSERT INTO locations(location_id,street_address,postal_code,city,state_province,country_id) VALUES (2500,'Magdalen Centre, The Oxford Science Park','OX9 9ZB','Oxford','Oxford','UK');
INSERT INTO locations(location_id,street_address,postal_code,city,state_province,country_id) VALUES (2700,'Schwanthalerstr. 7031','80925','Munich','Bavaria','DE');
 
 INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (1,'Public Accountant',4200.00,9000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (2,'Accounting Manager',8200.00,16000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (3,'Administration Assistant',3000.00,6000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (4,'President',20000.00,40000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (5,'Administration Vice President',15000.00,30000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (6,'Accountant',4200.00,9000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (7,'Finance Manager',8200.00,16000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (8,'Human Resources Representative',4000.00,9000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (9,'Programmer',4000.00,10000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (10,'Marketing Manager',9000.00,15000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (11,'Marketing Representative',4000.00,9000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (12,'Public Relations Representative',4500.00,10500.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (13,'Purchasing Clerk',2500.00,5500.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (14,'Purchasing Manager',8000.00,15000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (15,'Sales Manager',10000.00,20000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (16,'Sales Representative',6000.00,12000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (17,'Shipping Clerk',2500.00,5500.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (18,'Stock Clerk',2000.00,5000.00);
INSERT INTO jobs(job_id,job_title,min_salary,max_salary) VALUES (19,'Stock Manager',5500.00,8500.00);

INSERT INTO departments(department_id,department_name,location_id) VALUES (1,'Administration',1700);
INSERT INTO departments(department_id,department_name,location_id) VALUES (2,'Marketing',1800);
INSERT INTO departments(department_id,department_name,location_id) VALUES (3,'Purchasing',1700);
INSERT INTO departments(department_id,department_name,location_id) VALUES (4,'Human Resources',2400);
INSERT INTO departments(department_id,department_name,location_id) VALUES (5,'Shipping',1500);
INSERT INTO departments(department_id,department_name,location_id) VALUES (6,'IT',1400);
INSERT INTO departments(department_id,department_name,location_id) VALUES (7,'Public Relations',2700);
INSERT INTO departments(department_id,department_name,location_id) VALUES (8,'Sales',2500);
INSERT INTO departments(department_id,department_name,location_id) VALUES (9,'Executive',1700);
INSERT INTO departments(department_id,department_name,location_id) VALUES (10,'Finance',1700);
INSERT INTO departments(department_id,department_name,location_id) VALUES (11,'Accounting',1700);

INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (100,'Steven','King','steven.king@sqltutorial.org','515.123.4567','1987-06-17',4,24000.00,NULL,9);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (101,'Neena','Kochhar','neena.kochhar@sqltutorial.org','515.123.4568','1989-09-21',5,17000.00,100,9);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (102,'Lex','De Haan','lex.de haan@sqltutorial.org','515.123.4569','1993-01-13',5,17000.00,100,9);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (103,'Alexander','Hunold','alexander.hunold@sqltutorial.org','590.423.4567','1990-01-03',9,9000.00,102,6);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (104,'Bruce','Ernst','bruce.ernst@sqltutorial.org','590.423.4568','1991-05-21',9,6000.00,103,6);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (105,'David','Austin','david.austin@sqltutorial.org','590.423.4569','1997-06-25',9,4800.00,103,6);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (106,'Valli','Pataballa','valli.pataballa@sqltutorial.org','590.423.4560','1998-02-05',9,4800.00,103,6);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (107,'Diana','Lorentz','diana.lorentz@sqltutorial.org','590.423.5567','1999-02-07',9,4200.00,103,6);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (108,'Nancy','Greenberg','nancy.greenberg@sqltutorial.org','515.124.4569','1994-08-17',7,12000.00,101,10);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (109,'Daniel','Faviet','daniel.faviet@sqltutorial.org','515.124.4169','1994-08-16',6,9000.00,108,10);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (110,'John','Chen','john.chen@sqltutorial.org','515.124.4269','1997-09-28',6,8200.00,108,10);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (111,'Ismael','Sciarra','ismael.sciarra@sqltutorial.org','515.124.4369','1997-09-30',6,7700.00,108,10);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (112,'Jose Manuel','Urman','jose manuel.urman@sqltutorial.org','515.124.4469','1998-03-07',6,7800.00,108,10);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (113,'Luis','Popp','luis.popp@sqltutorial.org','515.124.4567','1999-12-07',6,6900.00,108,10);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (114,'Den','Raphaely','den.raphaely@sqltutorial.org','515.127.4561','1994-12-07',14,11000.00,100,3);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (115,'Alexander','Khoo','alexander.khoo@sqltutorial.org','515.127.4562','1995-05-18',13,3100.00,114,3);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (116,'Shelli','Baida','shelli.baida@sqltutorial.org','515.127.4563','1997-12-24',13,2900.00,114,3);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (117,'Sigal','Tobias','sigal.tobias@sqltutorial.org','515.127.4564','1997-07-24',13,2800.00,114,3);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (118,'Guy','Himuro','guy.himuro@sqltutorial.org','515.127.4565','1998-11-15',13,2600.00,114,3);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (119,'Karen','Colmenares','karen.colmenares@sqltutorial.org','515.127.4566','1999-08-10',13,2500.00,114,3);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (120,'Matthew','Weiss','matthew.weiss@sqltutorial.org','650.123.1234','1996-07-18',19,8000.00,100,5);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (121,'Adam','Fripp','adam.fripp@sqltutorial.org','650.123.2234','1997-04-10',19,8200.00,100,5);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (122,'Payam','Kaufling','payam.kaufling@sqltutorial.org','650.123.3234','1995-05-01',19,7900.00,100,5);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (123,'Shanta','Vollman','shanta.vollman@sqltutorial.org','650.123.4234','1997-10-10',19,6500.00,100,5);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (126,'Irene','Mikkilineni','irene.mikkilineni@sqltutorial.org','650.124.1224','1998-09-28',18,2700.00,120,5);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (145,'John','Russell','john.russell@sqltutorial.org',NULL,'1996-10-01',15,14000.00,100,8);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (146,'Karen','Partners','karen.partners@sqltutorial.org',NULL,'1997-01-05',15,13500.00,100,8);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (176,'Jonathon','Taylor','jonathon.taylor@sqltutorial.org',NULL,'1998-03-24',16,8600.00,100,8);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (177,'Jack','Livingston','jack.livingston@sqltutorial.org',NULL,'1998-04-23',16,8400.00,100,8);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (178,'Kimberely','Grant','kimberely.grant@sqltutorial.org',NULL,'1999-05-24',16,7000.00,100,8);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (179,'Charles','Johnson','charles.johnson@sqltutorial.org',NULL,'2000-01-04',16,6200.00,100,8);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (192,'Sarah','Bell','sarah.bell@sqltutorial.org','650.501.1876','1996-02-04',17,4000.00,123,5);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (193,'Britney','Everett','britney.everett@sqltutorial.org','650.501.2876','1997-03-03',17,3900.00,123,5);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (200,'Jennifer','Whalen','jennifer.whalen@sqltutorial.org','515.123.4444','1987-09-17',3,4400.00,101,1);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (201,'Michael','Hartstein','michael.hartstein@sqltutorial.org','515.123.5555','1996-02-17',10,13000.00,100,2);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (202,'Pat','Fay','pat.fay@sqltutorial.org','603.123.6666','1997-08-17',11,6000.00,201,2);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (203,'Susan','Mavris','susan.mavris@sqltutorial.org','515.123.7777','1994-06-07',8,6500.00,101,4);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (204,'Hermann','Baer','hermann.baer@sqltutorial.org','515.123.8888','1994-06-07',12,10000.00,101,7);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (205,'Shelley','Higgins','shelley.higgins@sqltutorial.org','515.123.8080','1994-06-07',2,12000.00,101,11);
INSERT INTO employees(employee_id,first_name,last_name,email,phone_number,hire_date,job_id,salary,manager_id,department_id) VALUES (206,'William','Gietz','william.gietz@sqltutorial.org','515.123.8181','1994-06-07',1,8300.00,205,11);

INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (1,'Penelope','Gietz','Child',206);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (2,'Nick','Higgins','Child',205);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (3,'Ed','Whalen','Child',200);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (4,'Jennifer','King','Child',100);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (5,'Johnny','Kochhar','Child',101);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (6,'Bette','De Haan','Child',102);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (7,'Grace','Faviet','Child',109);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (8,'Matthew','Chen','Child',110);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (9,'Joe','Sciarra','Child',111);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (10,'Christian','Urman','Child',112);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (11,'Zero','Popp','Child',113);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (12,'Karl','Greenberg','Child',108);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (13,'Uma','Mavris','Child',203);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (14,'Vivien','Hunold','Child',103);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (15,'Cuba','Ernst','Child',104);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (16,'Fred','Austin','Child',105);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (17,'Helen','Pataballa','Child',106);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (18,'Dan','Lorentz','Child',107);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (19,'Bob','Hartstein','Child',201);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (20,'Lucille','Fay','Child',202);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (21,'Kirsten','Baer','Child',204);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (22,'Elvis','Khoo','Child',115);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (23,'Sandra','Baida','Child',116);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (24,'Cameron','Tobias','Child',117);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (25,'Kevin','Himuro','Child',118);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (26,'Rip','Colmenares','Child',119);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (27,'Julia','Raphaely','Child',114);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (28,'Woody','Russell','Child',145);
INSERT INTO dependents(dependent_id,first_name,last_name,relationship,employee_id) VALUES (29,'Alec','Partners','Child',146);

--selection-all column show & projection-only what we want
--selection
select * from employees;
--selection by filtering rows
select * from employees where job_id=9;
--projecion
select first_name,last_name,phone_number from employees;
select first_name,last_name,phone_number,department_id from employees where department_id=2;

--questions

--display first 5 rows
select top 5* from employees;

-- display first_name, salary, phone_number and job_id for employees with job_id  9 and who get a salary above or equal 5000

SELECT first_name, salary, phone_number, job_id
FROM employees
WHERE job_id = 9 AND salary >= 5000;


select first_name, last_name ,salary, phone_number, job_id
into highSalaried FROM employees where salary>=10000;
--into is used for creating highSalaried it is new table name

--select employees from all depts except dept_id 9
SELECT *
FROM employees
WHERE department_id!=9;

--display employees who are hired on & after 1999 and arrange in descending order of hired date

SELECT first_name, hire_date 
FROM employees
where year(hire_date)>=1999
ORDER BY hire_date DESC;

--display employees who are hired in the year 2000,1995 and 1997
select first_name,hire_date from employees 
where year(hire_date) in(2000,1995,1997);

--display employees from departments 5,8,9,10
select first_name,hire_date from employees 
where department_id in(5,8,9,10);

--retrieve employees who have salaries between 9000 and 14000
SELECT *
FROM employees
WHERE salary BETWEEN 9000 AND 14000; 

-- select employees who salary above 10000 but only from department 8 and 10 

SELECT *
FROM employees where (department_id =8 or department_id =10)
and salary >= 10000;

--display employee first_name and last_name whose first_name starts with ka
SELECT first_name, last_name
FROM employees
WHERE first_name LIKE 'Ka%';

--% means 0 or many

--display employee first_name and last_name whose first_name starts with 'J' and ends with 'N'


SELECT first_name, last_name
FROM employees
WHERE first_name LIKE 'J%n';

-- display names that has 'a' as second character
SELECT first_name, last_name
FROM employees
WHERE first_name LIKE '_a%';


---- display first_name that has exactly 4 character
SELECT first_name, last_name
FROM employees
WHERE first_name LIKE '____';

--exact four sarts with j
SELECT first_name, last_name
FROM employees
WHERE first_name LIKE 'J___';

--find employees who work in dept 8,5,9 and have salary in range of 5000 anf 15000 and only a job id 3 or 1

SELECT *
FROM employees
WHERE department_id IN (8, 5, 9)
  AND salary BETWEEN 5000 AND 15000
  AND job_id=3 or job_id=1;


----------------------------------------------------------------------------
select first_name as fname, last_name as lname from employees;

--if has space in alias give ''/ you can skip keyword as aswell
select first_name as 'First Name' , last_name as 'Last Name' from employees;

--String funcs
select concat(first_name, ' . ',last_name) as 'Full Name' from employees;
--for concat +
select (first_name+' '+last_name) as 'Full Name' from employees;

select 2+3; 
--gives 5

select len(first_name) as 'Name Length' from employees;

--display the first names in the ascending oreder of the length of the name

SELECT first_name
FROM employees
ORDER BY LEN(first_name) ASC;

--length of ' Car'
--length of 'Car '
--length of ' Car '
select len(' Car')
--//4
select len('Car ')
--//3 only considers spaces in front
select len(' Car ')
--//4 only considers spaces in front

select left(first_name ,3) from employees 
--only 3 char from left
select right(first_name ,3) from employees 
--only 3 char from right last

select substring(email, 1,5) from employees;
select replace('hello','l','i');
--//heiio(string,letter that shoulg=d go,the replacement)

select reverse('SQL SERVER');
select UPPER('hai');
select lower('HaI');
select charindex('e','SQL SERVER')
--//6 gives the first occurence case insensitive

select len(trim(' car  '));
--//3 removes spaces trim

select format(1234567.89,'N0') as FormattedNumber; 
--dollar
select first_name,format(salary,'C','en_us') from employees

--rupees

select first_name,format(salary,'C','en_in') from employees
--euro
select first_name,format(salary,'C','de_de') from employees



select year(hire_date) from employees;
select month(hire_date) from employees;
select day(hire_date) from employees;

select getdate() as 'Today';
create table demo(id int , trans_time datetime);
insert into demo values(1,getdate());

select * from demo;

--people in different time zone
select sysdatetime()

--after 5 days from today 
select getdate() as issueDate, DATEADD(day, 5 , getdate() ) as 'Last Date';

--after 5 months from today 
select getdate() as issueDate, DateADD(month, 5 , getdate() ) as 'Last Date';

select hire_date, dateadd(year,10,hire_date) from employees; 


--part of date
select datepart(year,hire_date) from employees;

select datepart(year,'2025-04-23');


select datepart(month,'2025-04-23');


select datepart(day,'2025-04-23');

----------------------------------------------------------------------------------------------
--select datepart(week,'2025-04-23');

select datediff(year,hire_date,getdate()) experience from employees

--valid date format
select isdate('2024-02-30')
--invalid date format
select isdate('2024-19-23')

select DATENAME(month,'2024-03-30')

select DATENAME(day,'2024-03-30')

--dd mm yyyy  hh is case sensitive has different meanings

select format(getdate(),'dd-MM-YYYY');


select format(getdate(),'dd-mm-yyyy');


select format(getdate(),'DD-mm-YYYY');

--correct format
select format(getdate(),'dd-MM-yyyy:hh:mm');


-- display the date one week before
SELECT DATEADD(day, -7, GETDATE()) AS OneWeekAgo;

--select datepart(week,'2007-04-21'),datepart(weekday,'2007-04-21');

--select datepart(week,Getdate()),datepart(weekday,getdate());

select datename(month,getdate())+' ,'+datename(weekday , getdate());

--end of month
select EOMONTH(getdate());

--Mathematical functions
select (6/100),		
cast(6 as decimal(1,0))/cast(100 as decimal(3,0)),
cast(6.0/100.0 as decimal(4,2));


select round(1234.1234,2);

select round(1234.1264,2);

---convert(target type,input))
select convert(varchar(10),1234);
select convert(date,'20250321',4)

select convert(date,'20250321 14:52:30:00')

select convert(int,123.45)
select convert(XML,'<root><name>Alice</name></root>')

select convert(varchar,'03242025',1);

select convert(date,'20-02-2024',1);


---------------------number functions
select ceiling(-1.23),
floor(-1.23),
ceiling(1.23),
floor(1.23);

select round(123.99,0,1),round(123.99,1,1),round
(123.99,-1,1)




create tabledemo(value1 int, value2 int, value3 int);
insert into demo(value1, value2 , value3) values (11,12,13)
insert into demo( value2 , value3) values (12,13)
insert into demo(value1, value3) values (11,13)

select * demo
select * demo where value1 is not null

select * demo where value1 is null

--is null function validate whether an expression is null and if so, replcaes the null with the given value

select ISNULL(value1,100) from demo
--replace null value  same datatype

create table demo2(contact1 varchar(10),conntact2 varchar(10), contact3 varchar(10))
insert into demo2(contact1,contact2,contact3) values('address1','address2','address3')
insert into demo2(contact2,contact3) values('address2','address3')
select isnull(contact1,'NA') from demo2
select* from demo2


--coalesce will return the first no null value

select * from demo2;
insert into demo2(contact1,contact3) values('address1','address3')
insert into demo2(contact3) values('address3')
insert into demo2 values(null,null,null);
select coalesce(contact1,contact2,contact3) from demo2


select coalesce(contact1,contact2,contact3.'NA') from demo2

------Aggregate fns

select min(salary) from employees; 
select max(salary) from employees; 
select avg(salary) from employees; 
select sum(salary) from employees; 

--display the total sum of salary obtained by each department
select department_id,sum(salary) frpm employees group bby departent_id;

select department_id,sum(salary) frpm employees group bby departent_id having department_id in (5,8,9);

select count(*) frpm employees;
select count(employee_id) from employees;


--display  total sum of salary obtained by each department ascending order
select sum(salary) as total from employees group by department_id order by total asc;

--display  total number of employees in each department
Select count(employee_id) from employees group by department_id;

----display  department_id and salary for which the sum of salary is highest
select department_id,max(sum(salary)) from employees group by department_id;

--display the name of the employees who gets the highest salary
select (first_name+lastname) from employees where salary= max(salary);
// refer photo fro answers\

--display the departmentid that exist
select distinct(department_id) from employees

select distinct(hire_date) from employees

declare @highestsalary numeric(12,2) ;
select @highestsalary=max (salary ) from employees;
select @highestsalary as HighestSalary;

select first_name,salary from employees where salary=@highestsalary;

--subquery

select first_name,salary from employees where salary=(select max(salary) from employees)



-- top level manager he doest no t have a manager to report so manager id is null 
SELECT *
FROM employees
WHERE manager_id IS NULL;



--display the first_name,contact details if phno or email or not found

select first_name , coalesce(phone_number, email,'NOT FOUND) as contact_details from employees;

--display the employee details who get salary below the average salary

select avg(salary) from employees
select first_name,salary from employees where salary<(select avg(salary) from employees) order by salary;


--display the employee details along with department name

select first_name,department_name, l.street_address,l.postal_code,l.state_province,city,c.country_name ,r.region_name
from employees e join departments d 
on e.department_id=d.department_id
join location l on d.location_id=l.location_id 
join countries c on l.country_id=c.country_id
join regions r on c.region_id=r.region_id;


--display employees first,last_name,depart name from sales department 

select e.first_name,e.last_name,d.department_name from employees e join department d on e.department_id=d.department_id where d.department_name='Sales';


--display employees first,last_name,depart name from sales department  from europe region

select e.first_name,e.last_name,d.department_name, from employees e join department d on e.department_id=d.department_id where d.department_name='Sales' and r.region_name='Europe';
//ss

--display employee fn,salary,dept name,country for employees who are from united kingdom and get a salary less than 8000

SELECT e.first_name, e.salary, d.department_name, e.country
FROM employees e
JOIN departments d ON e.department_id = d.department_id
WHERE e.country = 'United Kingdom' AND e.salary < 8000;

--find the average salary of employees in the department with department_name='Sales'

SELECT AVG(e.salary) AS average_salary
FROM employees e
JOIN departments d ON e.department_id = d.department_id
WHERE d.department_name = 'Sales';

--Write a query to retrieve the department names along with the average salary of employees in each department.
SELECT d.department_name, AVG(e.salary) AS average_salary
FROM employees e
JOIN departments d ON e.department_id = d.department_id
GROUP BY d.department_name;

--Find the department name and the number of employees in each department:
SELECT d.department_name, COUNT(e.employee_id) AS num_employees
FROM employees e
JOIN departments d ON e.department_id = d.department_id
GROUP BY d.department_name;

--Find the names of employees along with their job titles
SELECT e.first_name, e.last_name, e.job_title
FROM employees e;

--Write a query to calculate the total salary for employees in each region. Display the region_id, region_name, and the total salary.
SELECT r.region_id, r.region_name, SUM(e.salary) AS total_salary
FROM employees e
JOIN departments d ON e.department_id = d.department_id
JOIN locations l ON d.location_id = l.location_id
JOIN regions r ON l.region_id = r.region_id
GROUP BY r.region_id, r.region_name;











select * from