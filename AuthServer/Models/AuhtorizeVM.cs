﻿namespace AuthServer.Models
{
    public class AuthorizeVM
    {
        public string ApplicationName { get; set; }
        public string Scopes { get; set; }
        public string Button { get; set; }
    }
}
