using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal class Customer
    {
         int customerID;
         string firstName;
         string lastName;
         string emailAddress;
         string phoneNumber;
         string address;

        public int CustomerID {
            get { return customerID; }
            set { customerID = value; }
        }
        public string FirstName {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName {
            get { return lastName; }
            set { lastName = value; }
        }
        public string EmailAddress {
            get { return emailAddress; }
            set { emailAddress = value; }
        }
        public string PhoneNumber {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string Address {
            get { return address; }
            set { address = value; }
        }

        public Customer()
        {

        }

        public Customer(int customerID, string firstName, string lastName, string emailAddress, string phoneNumber, string address)
        {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.emailAddress = emailAddress;
            this.phoneNumber = phoneNumber;
            this.address = address;
        }

        public void DisplayCustomerDetails()
        {
            Console.WriteLine($"Customer ID: {CustomerID}");
            Console.WriteLine($"Name: {FirstName} {LastName}");
            Console.WriteLine($"Email: {EmailAddress}");
            Console.WriteLine($"Phone: {PhoneNumber}");
            Console.WriteLine($"Address: {Address}");
        }
    }
}
