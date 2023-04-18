class Validation {
  static String? validateEmail(String value) {
    const Pattern pattern = r'^[a-zA-Z0-9.]+@[a-zA-Z0-9]+\.[a-zA-Z]+';
    final RegExp regex = RegExp(pattern as String);
    if (!regex.hasMatch(value)) {
      return 'Please enter a valid email address.';
    } else {
      return null;
    }
  }

  static String? validatePassword(String value) {
    const Pattern pattern = r'^.{4,}$';
    final RegExp regex = RegExp(pattern as String);
    if (!regex.hasMatch(value)) {
      return 'Password must be at least 4 characters.';
    } else {
      return null;
    }
  }
}
