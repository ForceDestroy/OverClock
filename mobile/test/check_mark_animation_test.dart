import 'package:mobile/themes/check_mark_animation.dart';
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';

void main() {
  const color = Colors.blue;

  Widget makeTestableWidget() {
    return const MaterialApp(
      home: Scaffold(
        body: AnimatedCheck(
          animationColor: color,
        ),
      ),
    );
  }

  testWidgets('Check that AnimatedCheck renders', (WidgetTester tester) async {
    await tester.pumpWidget(makeTestableWidget());
    expect(find.byKey(const Key('animatedCircle')), findsOneWidget);
    expect(find.byKey(const Key('animatedCheckMark')), findsOneWidget);
    final animatedCircle = find.byKey(const Key('animatedCircle'));
    final animation = tester.widget<ScaleTransition>(animatedCircle).scale;
    expect(animation.status, AnimationStatus.forward);
  });

  testWidgets('Check that AnimatedCheck animation completes',
      (WidgetTester tester) async {
    await tester.pumpWidget(makeTestableWidget());
    expect(find.byKey(const Key('animatedCircle')), findsOneWidget);
    expect(find.byKey(const Key('animatedCheckMark')), findsOneWidget);
    await tester.pumpAndSettle(const Duration(milliseconds: 2000));
    expect(find.byKey(const Key('animatedCircle')), findsOneWidget);
    expect(find.byKey(const Key('animatedCheckMark')), findsOneWidget);
  });

  testWidgets('Check that AnimatedCheck is disposed ',
      (WidgetTester tester) async {
    await tester.pumpWidget(makeTestableWidget());
    expect(find.byType(AnimatedCheck), findsOneWidget);
    await tester.pumpWidget(Container());
    expect(find.byType(AnimatedCheck), findsNothing);
  });
  test('check that checkmark animation is not null when created', () {
    const AnimatedCheck checkMarkAnimation =
        AnimatedCheck(animationColor: Colors.green);
    expect(checkMarkAnimation, isNotNull);
  });
  test('check that checkmark animation is the right color when called', () {
    const AnimatedCheck checkMarkAnimation =
        AnimatedCheck(animationColor: color);
    expect(checkMarkAnimation.animationColor, color);
  });
}
