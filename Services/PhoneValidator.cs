namespace ScheduleWebApp.Services
{
    public class PhoneValidator
    {
        private const int MinPhoneDigits = 10;

        public string Normalize(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return phone;

            return new string(phone
                .Where(c => char.IsDigit(c) || c == '+')
                .ToArray());
        }

        public bool IsValid(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return true;

            var digitCount = phone.Count(char.IsDigit);
            return digitCount >= MinPhoneDigits;
        }
    }
}