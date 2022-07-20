using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            var bikes = new List<Bike>();
            // implement code here... 
            using (var conn = Connection)
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT b.Id AS BikeId, Brand, Color, OwnerId,o.Name AS OwnerName, Address, Email, Telephone, BikeTypeId, bt.Name AS BikeName
                        FROM Bike b
                        LEFT JOIN Owner o ON b.OwnerId = o.Id
                        LEFT JOIN BikeType bt ON b.BikeTypeId = bt.Id";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bikes.Add(new Bike()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BikeId")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telephone = reader.GetString(reader.GetOrdinal("Telephone")),
                                },
                                BikeType = new BikeType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("BikeName")),
                                },
                            });
                        }

                        return bikes;
                    }
                }                
            }
        }

        public Bike GetBikeById(int id)
        {
            Bike bike = null;
            //implement code here...
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT b.Id AS BikeId, Brand, Color, OwnerId,o.Name AS OwnerName, Address, Email, Telephone, BikeTypeId, bt.Name AS BikeName
                        FROM Bike b
                        LEFT JOIN Owner o ON b.OwnerId = o.Id
                        LEFT JOIN BikeType bt ON b.BikeTypeId = bt.Id
                        WHERE b.Id = @Id;

                        SELECT Id, Description, DateInitiated, DateCompleted
                        FROM WorkOrder
                        WHERE BikeId = @Id";

                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bike = new Bike()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BikeId")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Owner = new Owner()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telephone = reader.GetString(reader.GetOrdinal("Telephone")),
                                },
                                BikeType = new BikeType()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                    Name = reader.GetString(reader.GetOrdinal("BikeName")),
                                },
                            };

                            if (reader.NextResult())
                            {
                                var workOrders = new List<WorkOrder>();

                                while (reader.Read())
                                {                                    
                                    var workOrder = new WorkOrder()
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        Description = reader.GetString(reader.GetOrdinal("Description")),
                                        DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                        DateCompleted = reader.GetDateTime(reader.GetOrdinal("DateCompleted")),
                                    };

                                    workOrders.Add(workOrder);
                                    bike.WorkOrders = workOrders;
                                }
                            };
                        }

                        return bike;
                    }
                }
            }
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT COUNT (DISTINCT BikeId) AS BikeCount
                        FROM WorkOrder
                        WHERE DateCompleted IS NULL";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(reader.GetOrdinal("BikeCount"));
                        }
                    }
                }
            }

            return count;
        }
    }
}
