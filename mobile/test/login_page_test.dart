// This is a basic Flutter widget test.
//
// To perform an interaction with a widget in your test, use the WidgetTester
// utility in the flutter_test package. For example, you can send tap and scroll
// gestures. You can also use WidgetTester to find child widgets in the widget
// tree, read text, and verify that the values of widget properties are correct.

import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';

import 'package:mobile/login_page.dart';

void main() {
  testWidgets('Form validation is called when email and password filled',
      (WidgetTester tester) async {
    const LoginDemo loginPage = LoginDemo();
    const app =
        MediaQuery(data: MediaQueryData(), child: MaterialApp(home: loginPage));
    await tester.pumpWidget(app);

    final Finder email = find.byKey(const Key('email'));
    final Finder pwd = find.byKey(const Key('password'));

    await tester.enterText(email, "email@email.com");
    await tester.enterText(pwd, "123456textemptynot");
    await tester.pump();

    final Finder formWidgetFinder = find.byType(Form);
    final Form formWidget = tester.widget(formWidgetFinder) as Form;
    final GlobalKey<FormState> formKey = formWidget.key as GlobalKey<FormState>;

    expect(formKey.currentState!.validate(), isTrue);
  });

  testWidgets(
      '"processing data" snackbar called when '
      'email and password filled and form is submitted',
      (WidgetTester tester) async {
    const LoginDemo loginPage = LoginDemo();
    const app =
        MediaQuery(data: MediaQueryData(), child: MaterialApp(home: loginPage));
    await tester.pumpWidget(app);

    final Finder email = find.byKey(const Key('email'));
    final Finder pwd = find.byKey(const Key('password'));

    await tester.enterText(email, "email@email.com");
    FocusManager.instance.primaryFocus?.unfocus();

    await tester.enterText(pwd, "123456textemptynot");
    FocusManager.instance.primaryFocus?.unfocus();
    await tester.ensureVisible(find.byKey(const Key('signIn')));
    await tester.tap(find.byKey(const Key('signIn')));
    await tester.pump();
  });
}
