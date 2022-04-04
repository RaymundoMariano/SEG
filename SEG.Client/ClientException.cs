using System;

namespace SEG.Client
{
    public class ClientException : Exception
    {
		public ClientException(String mensagem, Exception inner)
			: base(mensagem, inner)
		{

		}

		public ClientException(String mensagem)
			: base(mensagem)
		{

		}
	}
}
