import type { Metadata } from "next";
import { Inter } from "next/font/google";
import { ThemeModeScript } from "flowbite-react";
import "./globals.css";
import classNames from "classnames";
import { ClientLayout } from "@/components/ClientLayout";

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
        <ClientLayout>{children}</ClientLayout>
      </body>
    </html>
  );
}
