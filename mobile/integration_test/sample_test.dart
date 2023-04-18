import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:integration_test/integration_test.dart';
import 'package:mobile/main.dart' as app;

void main() {
  group('App Test', () {
    IntegrationTestWidgetsFlutterBinding.ensureInitialized();
    testWidgets("Login test", (tester) async {
      app.main();
      await tester.pumpAndSettle();
      final emailField = find.byType(TextField).first;
      final passwordField = find.byType(TextField).last;
      final loginButton = find.byType(ElevatedButton).first;

      await tester.enterText(emailField, "test4@gmail.com");
      await tester.enterText(passwordField, "password");
      await tester.pumpAndSettle();

      await tester.ensureVisible(find.byType(ElevatedButton));
      await tester.pumpAndSettle();
      await tester.tap(loginButton);
      await tester.pumpAndSettle(const Duration(seconds: 3));

      final job = find.text('My Job').first;
      final calendar = find.text('Calendar').last;
      final profile = find.text('Profile').first;
      final home = find.text('Home');
      await tester.pumpAndSettle();

      await tester.tap(job);
      await tester.pumpAndSettle();

      await tester.tap(calendar);
      await tester.pumpAndSettle();

      await tester.tap(profile);
      await tester.pumpAndSettle();

      await tester.drag(
          find.byType(SingleChildScrollView), const Offset(-200.0, -200.0));
      await tester.pumpAndSettle(const Duration(seconds: 2));

      final editProfile = find.text('Edit Profile').first;
      final nameField = find.byKey(const Key('name'));
      final numberField = find.byKey(const Key('number'));
      // final changeEmail = find.byType(ElevatedButton).last;
      // final changePassword = find.byType(TextField);
      final saveButton = find.text('Save');
      await tester.pumpAndSettle();

      await tester.ensureVisible(find.text('Edit Profile'));
      await tester.pumpAndSettle();
      await tester.tap(editProfile);
      await tester.pumpAndSettle(const Duration(seconds: 2));

      await tester.enterText(nameField, "Danny");
      await tester.pumpAndSettle(const Duration(seconds: 2));

      await tester.enterText(numberField, "5142320212");
      await tester.pumpAndSettle();

      await tester.ensureVisible(find.text('Save'));
      await tester.pumpAndSettle();
      await tester.tap(saveButton);
      await tester.pumpAndSettle(const Duration(seconds: 2));

      final theme2 = find.byKey(const Key('pinkTheme'));
      final theme3 = find.byKey(const Key('greenTheme'));
      final theme4 = find.byKey(const Key('orangeTheme'));
      final theme1 = find.byKey(const Key('blueTheme'));
      await tester.pumpAndSettle();

      await tester.drag(
          find.byType(SingleChildScrollView), const Offset(-200.0, -200.0));
      await tester.pumpAndSettle(const Duration(seconds: 2));

      await tester.tap(theme2);
      await tester.pumpAndSettle();

      await tester.tap(theme3);
      await tester.pumpAndSettle();

      await tester.tap(theme4);
      await tester.pumpAndSettle();

      await tester.tap(theme1);
      await tester.pumpAndSettle();

      await tester.tap(home);
      await tester.pumpAndSettle();
    });
  });
}
