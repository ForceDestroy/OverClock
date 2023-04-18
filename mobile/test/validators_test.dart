import 'package:flutter_test/flutter_test.dart';
import 'package:mobile/utils/validation.dart';

void main() {
  //Validator unit tests
  test('empty email test', () {
    final String? result = Validation.validateEmail('');
    expect(result, 'Please enter a valid email address.');
  });

  test('valid email test', () {
    final String? result = Validation.validateEmail('mimi@gmail.com');
    expect(result, null);
  });

  test('empty password test', () {
    final String? result = Validation.validatePassword('');
    expect(result, 'Password must be at least 4 characters.');
  });

  test('valid password test', () {
    final String? result = Validation.validatePassword('pAssw0rd');
    expect(result, null);
  });
}
