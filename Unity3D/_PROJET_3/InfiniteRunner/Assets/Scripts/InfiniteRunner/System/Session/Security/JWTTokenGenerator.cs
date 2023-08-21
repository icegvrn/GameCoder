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

public class JWTTokenGenerator 
{

    public string GenerateToken(int id, string username)
    {
        int date = (int)DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds();
        // Création des claims (informations à inclure dans le token)
        var payload = new Dictionary<string, object>
        {
            { "id", id },
            { "username", username },
            { "exp", date }
        };

        // Création de l'encodeur JWT
        IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); 
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

        // Génération du token
        string token = encoder.Encode(payload, DBConstant.JWTSecret);

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
            string decodedJson = decoder.Decode(token, DBConstant.JWTSecret, verify: true);

            JwtPayload payload = JsonUtility.FromJson<JwtPayload>(decodedJson);

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