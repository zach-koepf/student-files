﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;


namespace Capstone.DAO
{
    public class CardSqlDao : ICardDao
    {
        private readonly string connectionString;
        public CardSqlDao(string connString)
        {
            connectionString = connString;
        }

        public List<Card> GetAllCards()
        {
            List<Card> cardList = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT * FROM card", conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    cardList = new List<Card>();
                    while (reader.Read())
                    {
                        cardList.Add(CreateCardFromReader(reader));
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return cardList;
        }

        public Card GetCard(int cardId)
        {
            // establish the SQL connection
            Card card = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM card WHERE textcard_id = @card_id;", conn);
                cmd.Parameters.AddWithValue("@card_id", cardId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    card = CreateCardFromReader(reader); // create a card object from the information passed in by the database
                }
            }

            return card;
        }

        public Card CreateCard(Card card)
        {
            int newCardId;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO card (textcard_id, front, back, card_keywords, " +
                                                "deck_id)" +
                                                "OUTPUT INSERTED.textcard_id " +
                                                "VALUES (@card_id, @front, @back, @card_keywords, " +
                                                "@deck_id);", conn);
                cmd.Parameters.AddWithValue("@card_id", card.CardId);
                cmd.Parameters.AddWithValue("@front", card.Front);
                cmd.Parameters.AddWithValue("@back", card.Back);
                cmd.Parameters.AddWithValue("@card_keywords", card.CardKeywords);
                cmd.Parameters.AddWithValue("@deck_id", card.CardId);


                newCardId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return GetCard(newCardId);
        }

        public Card UpdateCard(int cardId, Card card)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UPDATE " +
                                                "SET textcard_id = @card_id, front = @front, back = @back, card_keywords = @card_kewords, deck_id = @deck_id " +
                                                "WHERE textcard_id = @card_id;", conn);
                cmd.Parameters.AddWithValue("@deck_id", card.DeckId);
                cmd.Parameters.AddWithValue("@card_id", card.CardId);
                cmd.Parameters.AddWithValue("@front", card.Front);
                cmd.Parameters.AddWithValue("@back", card.Back);
                cmd.Parameters.AddWithValue("@card_kewords", card.CardKeywords);


                cmd.ExecuteNonQuery();
            }

            Card checkCard = GetCard(cardId);


            return checkCard;
        }

        public bool DeleteCard(int cardId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM card WHERE textcard_id = @card_id;", conn);
                cmd.Parameters.AddWithValue("@card_id", cardId);

                cmd.ExecuteNonQuery();
            }

            // If there is still information at the location of this deck, it was not deleted properly
            if (GetCard(cardId) != null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private Card CreateCardFromReader(SqlDataReader reader)
        {
            Card card = new Card();
            card.DeckId = Convert.ToInt32(reader["deck_id"]);
            card.CardId = Convert.ToInt32(reader["textcard_id"]);
            card.Front = Convert.ToString(reader["front"]);
            card.Back = Convert.ToString(reader["back"]);
            card.CardKeywords = Convert.ToString(reader["card_keywords"]);


            return card;
        }
    }


}