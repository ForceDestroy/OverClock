import 'package:flutter/material.dart';

class AnimatedCheck extends StatefulWidget {
  final Color animationColor;
  const AnimatedCheck({Key? key, required this.animationColor})
      : super(key: key);

  @override
  // ignore: library_private_types_in_public_api
  _AnimatedCheckState createState() => _AnimatedCheckState();
}

class _AnimatedCheckState extends State<AnimatedCheck>
    with TickerProviderStateMixin {
  late AnimationController scaleController = AnimationController(
      duration: const Duration(milliseconds: 1500), vsync: this);
  late Animation<double> scaleAnimation =
      CurvedAnimation(parent: scaleController, curve: Curves.elasticOut);
  late AnimationController checkController = AnimationController(
      duration: const Duration(milliseconds: 1500), vsync: this);
  late Animation<double> checkAnimation =
      CurvedAnimation(parent: checkController, curve: Curves.linear);

  @override
  void initState() {
    super.initState();
    scaleController.addStatusListener((status) {
      if (status == AnimationStatus.completed) {
        checkController.forward();
      }
    });
    scaleController.forward();
  }

  @override
  void dispose() {
    scaleController.dispose();
    checkController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    const double circleSize = 100;
    const double iconSize = 100;

    return Stack(
      children: [
        ScaleTransition(
          key: const Key('animatedCircle'),
          scale: scaleAnimation,
          child: Container(
            height: circleSize,
            width: circleSize,
            decoration: BoxDecoration(
              color: widget.animationColor,
              shape: BoxShape.circle,
            ),
          ),
        ),
        ScaleTransition(
          key: const Key('animatedCheckMark'),
          scale: scaleAnimation,
          child: const Icon(Icons.check_rounded,
              color: Color.fromARGB(255, 255, 255, 255), size: iconSize),
        ),
      ],
    );
  }
}
