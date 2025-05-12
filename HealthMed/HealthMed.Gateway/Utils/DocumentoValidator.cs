namespace HealthMed.Gateway.Utils
{
    public static class DocumentoValidator
    {
        public static bool IsCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11)
                return false;

            if (new string(cpf[0], 11) == cpf)
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static bool IsCrm(string crm)
        {
            if (string.IsNullOrEmpty(crm))
                return false;

            crm = new string(crm.Where(char.IsDigit).ToArray());

            return crm.Length >= 5 && crm.Length <= 7;
        }

        public static DocumentoTipo DeterminarTipoDocumento(string documento)
        {
            if (string.IsNullOrEmpty(documento))
                return DocumentoTipo.Invalido;

            documento = new string(documento.Where(char.IsDigit).ToArray());

            if (IsCpf(documento))
                return DocumentoTipo.Cpf;

            if (IsCrm(documento))
                return DocumentoTipo.Crm;

            return DocumentoTipo.Outro;
        }
    }

    public enum DocumentoTipo
    {
        Cpf,
        Crm,
        Outro,
        Invalido
    }
}