use hexaware;
create table student1(sid int primary key identity,sname varchar(20),city varchar(20));

insert into student1(sname,city) values('Alice','Chennai');
insert into student1(sname,city) values('Bob','Mumbai');
insert into student1(sname,city) values('Joe','Pune');

select * from student1;
delete from student1 where sname='Joe';
insert into student1(sname,city) values('Joe','Pune');

--if you delete the data in 3 the next data you insert will get only 4 it wont give the number that got deleted


--declare identity for a non primary key column
create table student2 (sid int primary key,sname varchar(20),rollNo int identity);
insert into student2(sid,sname) values(101,'Alice');
insert into student2(sid,sname) values(102,'Bob');
insert into student2(sid,sname) values(103,'Leo');
select * from student2;