//Token Builder
        public AuthenticationResponse BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", userCredentials.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("GVL2vLyWx11bArSNTAe1RhtkYOWhF8z8RhtkYOWhF8z8"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(7);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: cred);

            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

//Startup file

//Identity User
         services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

///JTW
         services.AddAuthentication(o => {
             o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(
             options=> {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = false,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["keyjwt"])),
                     ClockSkew = TimeSpan.Zero
                 };
             }
             );

///Cors
         services.AddCors(options => {
             var FrontEndURL = Configuration.GetValue<string>("frontend_url");
             options.AddDefaultPolicy(builder =>
             {
                 builder.WithOrigins(FrontEndURL).AllowAnyMethod().AllowAnyHeader();
             }
           );
         });
     }

app.UseCors();

app.UseAuthentication();
