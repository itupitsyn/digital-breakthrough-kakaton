"use client";

import { DarkThemeToggle, Navbar, TextInput } from "flowbite-react";
import Logo from "../assets/extremely-serious-blowing-sun.svg";

import { FC } from "react";
import { BiSearch } from "react-icons/bi";
import { useSearchContext } from "@/contexts/searchContext";
import { LoginButton } from "./LoginButton";

export const UltraNavbar: FC = () => {
  const { onSearch } = useSearchContext();

  return (
    <Navbar className="sticky top-0 z-10 [&>div]:gap-2">
      <Logo className="-order-1 size-14" />
      <div className="dark:text-white">Ультравзрывная подборочка</div>
      <TextInput icon={BiSearch} placeholder="Поиск" className="max-w-[500px] grow" onChange={onSearch} />
      <div className="-order-1 flex items-center gap-2 md:order-1">
        <DarkThemeToggle />
        <LoginButton />
      </div>
    </Navbar>
  );
};
