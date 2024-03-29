﻿using AutoMapper.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webapi.Models;

namespace webapi.Jwt
{

	public interface IJwtGenerator
	{
		string CreateToken(ApplicationUser user);
	}

	public class JwtGenerator : IJwtGenerator
	{
		private readonly SymmetricSecurityKey _key;

		public JwtGenerator()
		{
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));
		}

		public string CreateToken(ApplicationUser user)
		{
			var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, user.UserName) };

			var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = credentials
			};
			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}