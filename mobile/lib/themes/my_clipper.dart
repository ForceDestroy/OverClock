import 'package:flutter/material.dart';

class MyClipper extends CustomClipper<Path> {
  @override
  Path getClip(Size size) {
    final path = Path();
    path.lineTo(0, 200);
    path.quadraticBezierTo(
        size.width / 6, size.height / 4, size.width / 4.5, 120);
    path.quadraticBezierTo(size.width / 3.6, 80, size.width / 2.3, 100);
    path.quadraticBezierTo(size.width / 1.5, 120, size.width / 1.5, 0);
    path.lineTo(size.width, 0);
    path.close();
    return path;
  }

  @override
  bool shouldReclip(CustomClipper<Path> oldClipper) {
    return false;
  }
}

//Class that creates custom shape for profile header
class Semicircle extends CustomClipper<Path> {
  @override
  Path getClip(Size size) {
    final double w = size.width;
    final double h = size.height;

    final path = Path();

    path.lineTo(0, h - 120);
    path.quadraticBezierTo(
      w * 0.5,
      h,
      w,
      h - 120,
    );
    path.lineTo(w, 0);
    path.close();

    return path;
  }

  @override
  bool shouldReclip(CustomClipper<Path> oldClipper) {
    return false;
  }
}

class HomePageSemicircle extends CustomClipper<Path> {
  @override
  Path getClip(Size size) {
    final double w = size.width;
    final double h = size.height;

    final path = Path();

    path.lineTo(0, h / 4.5);
    path.quadraticBezierTo(w / 2, h / 3, w, h / 4.5);
    path.lineTo(w, 0);
    path.close();

    return path;
  }

  @override
  bool shouldReclip(CustomClipper<Path> oldClipper) {
    return false;
  }
}
