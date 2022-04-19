namespace rulestest
{
    public static class Utils
    {
        public static bool StartsWithA(string input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return false;
            }

            return input.ToLower().StartsWith("a");
        }

        public static bool MustContainAll(string idList, List<string> listOfIds) {
            if (listOfIds == null || !listOfIds.Any() || string.IsNullOrWhiteSpace(idList)) {
                return false;
            }

            var hasAllIds = idList.Split(',').ToList().Select(x => x.Trim()).ToList().All(value => listOfIds.Contains(value));
            return hasAllIds;
        }
    }
}