using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DisconnectedDemo.Models;

using DisconnectedDemo.Data;

namespace DisconnectedDemo.Data
{
    public class ProductDaoImpl : IProductDao
    {
        SqlDataAdapter adapter;
        DataSet dataSet;
        string tableName = "products";


        public ProductDaoImpl()
        {
            string query = "select * from products";
            adapter = DBUtil.GetAdapter(query);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            dataSet = new DataSet();
            adapter.Fill(dataSet, tableName);

            //Set the primary key fror the dataTable in the Dtaset

            DataTable table = dataSet.Tables[tableName];
            table.PrimaryKey = new DataColumn[] { table.Columns["product_id"] };


        }

        public void AddProduct(Product product)
        {
            DataTable table = dataSet.Tables[tableName];
            DataRow newRow = table.NewRow();
            newRow["product_id"] = product.ProductId;
            newRow["product_name"] = product.ProductName;
            newRow["price"] = product.Price;
            newRow["category"] = product.Category;
            table.Rows.Add(newRow);

        }

        public DataTable GetAllProducts()
        {
            return dataSet.Tables[tableName];
        }

        public void UpdateProduct(int id, decimal newPrice)
        {
            DataTable table = dataSet.Tables[tableName];
            DataRow row = table.Rows.Find(id);
            if (row != null)
            {
                row["price"] = newPrice;
            }
        }

        public void DeleteProduct(int id)
        {
            DataTable table = dataSet.Tables[tableName];
            DataRow row = table.Rows.Find(id);
            if (row != null)
            {
                row.Delete();
            }
        }

        public void SaveChanges()
        {
            adapter.Update(dataSet, tableName);//opens conection and updates database
        }



    }
}
