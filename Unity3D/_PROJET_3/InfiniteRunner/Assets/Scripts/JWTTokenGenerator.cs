using System.Collections.Generic;
using UnityEngine;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using JWT.Exceptions;
using System;

[System.Serializable]
public class JwtPayload
{
    public int id;
    public string username;
    public int exp;
}

public class JWTTokenGenerator : MonoBehaviour
{
    void Start()
    {

    }


    public string GenerateToken(int id, string username)
    {
        int date = (int)DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds();
        // Cr�ation des claims (informations � inclure dans le token)
        var payload = new Dictionary<string, object>
        {
            { "id", id },
            { "username", username },
            { "exp", date }
        };

        // Cr�ation de l'encodeur JWT
        IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // Utilisez l'algorithme appropri�
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        // G�n�ration du token
        string token = encoder.Encode(payload, DBConstant.JWTSecret);

        // Affichage du token (� des fins de d�monstration)
        Debug.Log("Generated Token: " + token);

        return token;
    }

    public bool JWTTokenVerify(int userId, string token)
    {
        IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
        IJwtValidator validator = new JwtValidator(serializer, new UtcDateTimeProvider());
        IJwtAlgorithm decodingAlgorithm = new HMACSHA256Algorithm();
        IJwtDecoder decoder = new JwtDecoder(serializer, validator, new JwtBase64UrlEncoder(), algorithm);

        try
        {
            Debug.Log("Avant le d�code");
            string decodedJson = decoder.Decode(token, DBConstant.JWTSecret, verify: true);
            Debug.Log("Decoded Token Payload: " + decodedJson);

            JwtPayload payload = JsonUtility.FromJson<JwtPayload>(decodedJson);

            Debug.Log("User ID: " + payload.id);
            Debug.Log("Username: " + payload.username);
            Debug.Log("Date expiration du token: " +payload.exp);

      
            if (payload.id == userId && (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() < payload.exp)
            {
                return true;
            }
            else
            {
                Debug.Log("Votre token est invalide");
                return false;
            }

     
        }
        catch (TokenExpiredException)
        {
            Debug.LogError("Token has expired.");
            return false;
        }
        catch (SignatureVerificationException)
        {
            Debug.LogError("Token signature is invalid.");
            return false;
        }
    }
}