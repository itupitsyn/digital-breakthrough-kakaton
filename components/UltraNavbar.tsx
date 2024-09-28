"use client";

import { DarkThemeToggle, Navbar, TextInput } from "flowbite-react";
import Logo from "../assets/extremely-serious-blowing-sun.svg";

import { FC } from "react";
import { BiSearch } from "react-icons/bi";

export const UltraNavbar: FC = () => {
  return (
    <Navbar className="sticky top-0 z-10 [&>div]:gap-2">
      <Logo className="size-14" />
      <div className="dark:text-white">Ультравзрывной подбор очка</div>
      <TextInput icon={BiSearch} placeholder="Поиск" className="max-w-[500px] grow" />
      <DarkThemeToggle />
    </Navbar>
  );
};
