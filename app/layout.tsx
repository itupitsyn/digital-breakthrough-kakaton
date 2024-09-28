import type { Metadata } from "next";
import { Inter } from "next/font/google";
import { ThemeModeScript } from "flowbite-react";
import "./globals.css";
import classNames from "classnames";
import { UltraNavbar } from "../components/UltraNavbar";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Ультравзрыв Inc",
  description: "Очень серьёзный ультравзрыв!!",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="ru">
      <head>
        <ThemeModeScript />
      </head>
      <body className={classNames(inter.className, "dark:bg-gray-800 text-gray-900 dark:text-white")}>
        <div className="container min-h-svh pb-12">
          <UltraNavbar />
          {children}
        </div>
      </body>
    </html>
  );
}
