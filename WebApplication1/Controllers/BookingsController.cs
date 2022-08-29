using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BookingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet("get")]
        public JsonResult Get(string first_name, string last_name, string starts_at, string booked_at, string name, int confirmed)
        {
            /*TO_TIMESTAMP(b.starts_at,'YYYY/MM/DD HH24:MI:SS') + b.booked_for * INTERVAL '1 day'*/
            string query = @"
                select u.first_name,u.last_name,u.email,u.phone,a.name,a.address,a.zip_code,a.city,a.country,b.starts_at,b.booked_at,b.confirmed
                from users u INNER JOIN bookings b on u.id = b.user_id INNER JOIN appartments a on a.id = b.apartment_id
                where b.id is not null";

            DataTable table = new DataTable();
            string sqlDataSoruce = _configuration.GetConnectionString("postgresql");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon=new NpgsqlConnection(sqlDataSoruce))
            {
                myCon.Open();

                if (!String.IsNullOrEmpty(first_name))
                {
                    query += "  and u.first_name=@first_name";

                }
                if (!string.IsNullOrEmpty(last_name))
                {
                    query += " and u.last_name=@last_name";


                }
                if (!string.IsNullOrEmpty(starts_at))
                {
                    query += " and b.starts_at=@starts_at";

                }
                if (!string.IsNullOrEmpty(booked_at))
                {
                    query += " and b.booked_at=@booked_at";

                }
                if (!string.IsNullOrEmpty(name))
                {
                    query += " and a.name=@name";
                    
                }
                if (!string.IsNullOrEmpty(confirmed.ToString()))
                {
                    query += " and confirmed=@confirmed";
                   
                }


                NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon);


                if (!String.IsNullOrEmpty(first_name))
                {
                    myCommand.Parameters.AddWithValue("@first_name", first_name);

                }
                if (!string.IsNullOrEmpty(last_name))
                {
                    myCommand.Parameters.AddWithValue("@last_name", last_name);


                }
                if (!string.IsNullOrEmpty(starts_at))
                {
                    myCommand.Parameters.AddWithValue("@starts_at", starts_at);

                }
                if (!string.IsNullOrEmpty(booked_at))
                {
                    myCommand.Parameters.AddWithValue("@booked_at", booked_at);

                }
                if (!string.IsNullOrEmpty(name))
                {
                    myCommand.Parameters.AddWithValue("@name", name);

                }
                if (!string.IsNullOrEmpty(confirmed.ToString()))
                {

                    myCommand.Parameters.AddWithValue("@confirmed", confirmed);

                }
                
                myReader = myCommand.ExecuteReader();
                table.Load(myReader);

                myReader.Close();
                myCon.Close();


            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Bookings bookings)
        {
            string query = @"
                    Insert into bookings(id,user_id,starts_at,booked_at,booked_for,apartment_id,confirmed)
                    values ((select max(id) from bookings)+1,@user_id,@starts_at,@booked_at,@booked_for,@apartment_id,@confirmed)
            ";

            DataTable table = new DataTable();
            string sqlDataSoruce = _configuration.GetConnectionString("postgresql");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSoruce))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {
                  
                    myCommand.Parameters.AddWithValue("@user_id", bookings.user_id);
                    myCommand.Parameters.AddWithValue("@starts_at",bookings.starts_at);
                    myCommand.Parameters.AddWithValue("@booked_at", bookings.booked_at);
                    myCommand.Parameters.AddWithValue("@booked_for", bookings.booked_for);
                    myCommand.Parameters.AddWithValue("@apartment_id", bookings.apartment_id);
                    myCommand.Parameters.AddWithValue("@confirmed", bookings.confirmed);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }

            }

            return new JsonResult("Added successfully");
        }

        [HttpPut]
        public JsonResult Put(Bookings bookings)
        {
            string query = @"
                    update bookings
                    set user_id=@user_id,starts_at=@starts_at,booked_at=@booked_at,booked_for=@booked_for,apartment_id=@apartment_id,confirmed=@confirmed
                    where id=@id
            ";

            DataTable table = new DataTable();
            string sqlDataSoruce = _configuration.GetConnectionString("postgresql");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSoruce))
            {
                myCon.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                {

                    myCommand.Parameters.AddWithValue("@user_id", bookings.user_id);
                    myCommand.Parameters.AddWithValue("@starts_at", bookings.starts_at);
                    myCommand.Parameters.AddWithValue("@booked_at", bookings.booked_at);
                    myCommand.Parameters.AddWithValue("@booked_for", bookings.booked_for);
                    myCommand.Parameters.AddWithValue("@apartment_id", bookings.apartment_id);
                    myCommand.Parameters.AddWithValue("@confirmed", bookings.confirmed);
                    myCommand.Parameters.AddWithValue("@id", bookings.id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }

            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete("delete")]
        public JsonResult Delete(int id, int confirmed)
        {
            if (confirmed == 1)
            {
                return new JsonResult("Onaylı confirmed değeri silinemez");
            }
            else
            {
                string query = @"
                   delete from bookings where id = @id and confirmed=0
                ";
                DataTable table = new DataTable();
                string sqlDataSoruce = _configuration.GetConnectionString("postgresql");
                NpgsqlDataReader myReader;
                using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSoruce))
                {
                    myCon.Open();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@id", id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }

                }

                return new JsonResult("Deleted Successfully");
            }
           
        }

    }
}
